open System
open Shutdown

let printResult (res: Result<_, _>) =
    match res with
    | Ok o -> printfn "OK(%O)" o
    | Error e -> printfn "Error(%O)" e

let startTimer (time: TimeSpan) =
    let option = ShutdownOption.create ShutdownAction.Shutdown CloseType.CloseWindowsWithoutSave (TimeInSecond.Time time)
    Shutdown.exec option

let startTimerDirectCreate (time: TimeSpan) =
    let option = { Action = ShutdownAction.Shutdown; WaitTime = TimeInSecond.Time time; CloseType = CloseType.CloseWindowsWithoutSave; Message = Message.Msg "Custom message"}
    Shutdown.exec option

let stopTimer() =
    Shutdown.stop()

[<EntryPoint>]
let main argv =
    startTimerDirectCreate (TimeSpan.FromMinutes(30.)) |> printResult
    stopTimer() |> printResult
    Console.ReadKey() |> ignore 
    0