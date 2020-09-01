namespace Shutdown

open System
open System.Collections.ObjectModel
open System.Diagnostics
open System.Text

[<Struct>]
type ShutdownAction =
    | Restart
    | Shutdown

[<Struct>]
type CloseType =
    | CloseWindowsWithoutSave
    | SoftWindowsClose

[<Struct>]
type TimeInSecond =
    | Time of TimeSpan
    | None

[<Struct>]
type Message =
    | Msg of string
    | None

type ShutdownOption = {
    Action: ShutdownAction
    WaitTime: TimeInSecond
    CloseType: CloseType
    Message: Message
}

[<Struct>]
type ExecuteInfo = {
    FileName: string
    Arguments: string list
}

module private Message =
    let inline format (msg: Message) =
        match msg with
        | Msg msg -> sprintf "/c \"%s\"" msg
        | _ -> ""

module private WaitTime =
    let format (time: TimeInSecond) =
        match time with
        | Time t -> sprintf "/t \"%d\"" (int t.TotalSeconds)
        | _ -> ""

module private CloseType =
    let inline format (closeType: CloseType) =
        match closeType with
            | CloseWindowsWithoutSave -> "/f"
            | SoftWindowsClose -> "/soft"
            
module private ShutdownAction =
    let inline format (action: ShutdownAction) =
        match action with
        | Restart -> "/r"
        | Shutdown -> "/s"
 
module private ExecuteInfo =
    let inline create arguments =
        { FileName = "shutdown.exe"; Arguments = arguments}
    
module ShutdownOption =
    let create action closeType waitTime =
        { Action = action; CloseType = closeType; WaitTime = waitTime; Message = Message.None }
            
module Shutdown =
    type Arguments = string list
    
    let inline private append (str: string) (builder: Arguments) =
        str :: builder
    
    let inline private appendMap<'a> (value: 'a) (map: 'a -> string) (builder: Arguments) =
        builder
        |> append (map value)
        
    let inline private appendMapNotEmpty<'a> (value: 'a) (map: 'a -> string) (builder: Arguments) =
        let mapValue = map value
        match mapValue with
        | "" | null -> builder
        | v -> builder |> append v
            
    let inline private asExecuteInfo (option: ShutdownOption) =
        list.Empty
            |> appendMap option (fun x -> ShutdownAction.format x.Action)
            |> appendMap option (fun x -> CloseType.format x.CloseType)
            |> appendMapNotEmpty option (fun x -> WaitTime.format x.WaitTime)
            |> appendMapNotEmpty option (fun x -> Message.format x.Message)
            |> ExecuteInfo.create
                  
    let private execExecuteInfo (info: ExecuteInfo) =
          try
            let startInfo = ProcessStartInfo()
            
            startInfo.FileName <- info.FileName
            startInfo.Arguments <- info.Arguments |> String.concat " "
               
            startInfo.Verb <- "runas"
            
            let proc = new Process()
            proc.StartInfo <- startInfo
            proc.Start() |> Ok
            
            with
                | Failure ex -> Error ex
                        
    let stop() =
        list.Empty
        |> append "/a"
        |> ExecuteInfo.create
        |> execExecuteInfo
            
    let exec (option: ShutdownOption) =
        option
        |> asExecuteInfo
        |> execExecuteInfo
        
    