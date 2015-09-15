#r @"../../packages/Http.fs.1.5.1/lib/net40/HttpClient.dll"

open System
open HttpClient

// web api endpoint
let baseUrl = "http://localhost:8083"

// very bare-bones api client
module Client =
    let createRequest baseUrl ``method`` path =
        HttpClient.createRequest ``method`` (sprintf "%s%s" baseUrl path)
        |> withHeader (ContentType "text/plain")
        |> withHeader (Accept "*/*")

    let get = createRequest baseUrl Get
    let post = createRequest baseUrl Post
    let put = createRequest baseUrl Put

    let askHello (ip:string) = 
        get "/" 
        |> HttpClient.withQueryStringItem { name = "fake_ip_addr"; value = ip }
        |> getResponse

Client.askHello "192.168.1.1"