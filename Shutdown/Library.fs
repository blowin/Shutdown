namespace Shutdown

open System
open System.Diagnostics

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
    private | Msg of string
    | None

[<Struct>]
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

module private CloseType =
    let format (closeType: CloseType) =
        match closeType with
        | CloseWindowsWithoutSave -> "/f"
        | _ -> ""

module Message =
    let createMsg (str: string) =
        if str = null || str.Length > 512 then Error "Сообщение не должно быть больше 512 символов"
        else Ok (Message.Msg str)

    let createNone() = Message.None

module private WaitTime =
    let format (time: TimeInSecond) =
        match time with
        | Time t -> sprintf "/t \"%d\"" (int t.TotalSeconds)
        | _ -> ""
            
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
        { Action = action; WaitTime = waitTime; CloseType = closeType; Message = Message.None }
            
module ShutdownLib =
    type Arguments = string list
    
    let inline private append (str: string) (builder: Arguments) =
        str :: builder
    
    let inline private appendMap<'a> (value: 'a) (map: 'a -> string) (builder: Arguments) =
        builder
        |> append (map value)
       
    let inline private appendMapNotEmpty<'a> (value: 'a) (map: 'a -> string) (builder: Arguments) =
        match (map value) with
        | "" | null -> builder
        | value -> builder |> append value
       
    let inline private formatMessage (msg: Message) =
        match msg with
        | Msg msg -> sprintf "/c \"%s\"" msg
        | _ -> ""

    let inline private asExecuteInfo (option: ShutdownOption) =
        list.Empty
            |> appendMap option (fun x -> ShutdownAction.format x.Action)
            |> appendMapNotEmpty option (fun x -> WaitTime.format x.WaitTime)
            |> appendMapNotEmpty option (fun x -> formatMessage x.Message)
            |> appendMapNotEmpty option (fun x -> CloseType.format x.CloseType)
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
        
    