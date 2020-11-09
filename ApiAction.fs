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
            printfn "[ApiClient] Get %s" key
            match Map.tryFind key data with
            | Some o ->
                this.TryCast<'a> key o
            | None ->
                Result.Failure [sprintf "Key %s not found" key]

        member this.Set (id:obj) (value:obj) = 
            let key = sprintf "%A" id
            printfn "[ApiClient] Set %s" key
            if key = "bad" then
                Result.Failure [sprintf "Bad Key %s" key]
            else
                data <- Map.add key value data
                Result.Success ()

        member this.Open() = 
            printfn "[ApiClient] Opening"

        member this.Close() =
            printfn "[ApiClient] Closing"

        interface System.IDisposable with
            member this.Dispose() =
                printfn "[ApiClient] Disposing"

    type ApiAction<'a> = ApiAction of (ApiClient -> 'a)

    let run apiClient (ApiAction action) =
        let resultOfAction = action apiClient
        resultOfAction

    let map f action = 
        let newAction apiClient = 
            let x = run apiClient action
            f x
        ApiAction newAction

    let retn x =
        let newAction apiClient =
            x
        ApiAction newAction

    let apply fAction xAction =
        let newAction apiClient = 
            let f = run apiClient fAction
            let x = run apiClient xAction
            f x
        ApiAction newAction

    let bind f xAction = 
        let newAction apiClient = 
            let x = run apiClient xAction
            run apiClient (f x)
        ApiAction newAction

    let execute apiAction = 
        use apiClient = new ApiClient()
        apiClient.Open()
        let result = run apiClient apiAction
        apiClient.Close()
        result
