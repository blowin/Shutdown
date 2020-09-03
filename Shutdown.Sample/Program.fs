open System
open Shutdown

let printResult (res: Result<_, _>) =
    match res with
    | Ok o -> printfn "OK(%O)" o
    | Error e -> printfn "Error(%O)" e

let startTimer (time: TimeSpan) =
    let option = ShutdownOption.create ShutdownAction.Shutdown CloseType.CloseWindowsWithoutSave (TimeInSecond.Time time)
    ShutdownLib.exec option

let startTimerDirectCreate (time: TimeSpan) =
    let msg = Message.createMsg "Custom message"
    match msg with
    | Ok msg -> 
        let option = { Action = ShutdownAction.Shutdown; WaitTime = TimeInSecond.Time time; CloseType = CloseType.CloseWindowsWithoutSave; Message = msg}
        ShutdownLib.exec option
    | Error err -> Error err

let stopTimer() =
    ShutdownLib.stop()

[<EntryPoint>]
let main argv =
    startTimerDirectCreate (TimeSpan.FromMinutes(30.)) |> printResult
    stopTimer() |> printResult
    Console.ReadKey() |> ignore 
    0