open System
open Shutdown

let printResult (res: Result<_, _>) =
    match res with
    | Ok o -> printfn "OK(%O)" o
    | Error e -> printfn "Error(%O)" e

let startTimer (time: TimeSpan) =
    let option = { Action = ShutdownAction.Shutdown; WaitTime = TimeInSecond.Time time; CloseType = CloseType.CloseWindowsWithoutSave}
    Shutdown.exec option

let stopTimer() =
    Shutdown.stop()

[<EntryPoint>]
let main argv =
    startTimer (TimeSpan.FromMinutes(30.)) |> printResult
    stopTimer() |> printResult
    Console.ReadKey() |> ignore 
    0