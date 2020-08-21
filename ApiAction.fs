namespace FunctionalPatterns

module ApiAction =

    type ApiClient() =
        static let mutable data = Map.empty<string, obj>

        member private this.TryCast<'a> key (value:obj) =
            match value with
            | :? 'a as a ->
                Result.Success a
            | _ ->
                let typeName = typeof<'a>.Name
                Result.Failure [sprintf "Can't cast value at %s to %s" key typeName]

        member this.Get<'a> (id:obj) = 
            let key = sprintf "%A" id
            printfn "[API] Get %s" key
            match Map.tryFind key data with
            | Some o ->
                this.TryCast<'a> key o
            | None ->
                Result.Failure [sprintf "Key %s not found" key]

        member this.Set (id:obj) (value:obj) = 
            let key = sprintf "%A" id
            printfn "[API] Set %s" key
            if key = "bad" then
                Result.Failure [sprintf "Bad Key %s" key]
            else
                data <- Map.add key value data
                Result.Success ()

        member this.Open() = 
            printfn "[API] Opening"

        member this.Close() =
            printfn "[API] Closing"

        interface System.IDisposable with
            member this.Dispose() =
                printfn "[API] Disposing"

    type ApiAction<'a> = ApiAction of (ApiClient -> 'a)

    let run api (ApiAction action) =
        let resultOfAction = action api
        resultOfAction

    let map f action = 
        let newAction api = 
            let x = run api action
            f x
        ApiAction newAction

    let retn x =
        let newAction api =
            x
        ApiAction newAction

    let apply fAction xAction =
        let newAction api = 
            let f = run api fAction
            let x = run api xAction
            f x
        ApiAction newAction

    let bind f xAction = 
        let newAction api = 
            let x = run api xAction
            run api (f x)
        ApiAction newAction

    let execute action = 
        use api = new ApiClient()
        api.Open()
        let result = run api action
        api.Close()
        result
