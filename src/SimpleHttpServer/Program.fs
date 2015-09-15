// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Threading
open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.Writers
open Suave.Web
open Suave.Types
open Suave.Http.RequestErrors
open Suave.Utils
open Suave.Http.Authentication
open RateLimiter

[<AutoOpen>]
module Utils = 
    let inline (|?) o d = defaultArg o d

// a module with some functions to start a web server to host the api
[<AutoOpen>]
module WebServer =
    let rateLimiter = new RateLimiter();

    let private app =
        setHeader "Content-Type" "text/plain"
        >>= choose [ request(fun r ->
                        let valid = 
                            match r.queryParam "fake_ip_addr" with
                            | Choice1Of2 (ip) -> rateLimiter.IsValidRequest("", DateTime.Now)
                            | _ -> false

                        if valid
                            then never
                            else ServerErrors.SERVICE_UNAVAILABLE "Please wait before submitting another request")
                     GET  >>= path "/" >>= OK "Hi!" 
                     RequestErrors.NOT_FOUND "Found no handlers" ]

    let startAsyc() =
        let cts = new CancellationTokenSource()
        startWebServerAsync { defaultConfig with cancellationToken = cts.Token } app
        |> snd
        |> fun server -> Async.StartAsTask(server, cancellationToken = cts.Token) |> ignore; cts 


[<EntryPoint>]
let main argv = 
    let cancel = startAsyc()
    ignore <| System.Console.ReadLine()
    ignore <| cancel.Dispose()
    0 // return an integer exit code
    
