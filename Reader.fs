namespace FunctionalPatterns

module Reader =

    type Environment() =
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
            printfn "[Environment] Get %s" key
            match Map.tryFind key data with
            | Some o ->
                this.TryCast<'a> key o
            | None ->
                Result.Failure [sprintf "Key %s not found" key]

        member this.Set (id:obj) (value:obj) = 
            let key = sprintf "%A" id
            printfn "[Environment] Set %s" key
            if key = "bad" then
                Result.Failure [sprintf "Bad Key %s" key]
            else
                data <- Map.add key value data
                Result.Success ()

        member this.Open() = 
            printfn "[Environment] Opening"

        member this.Close() =
            printfn "[Environment] Closing"

        interface System.IDisposable with
            member this.Dispose() =
                printfn "[Environment] Disposing"

    type Reader<'a> = Reader of (Environment -> 'a)

    let run environment (Reader action) =
        let resultOfAction = action environment
        resultOfAction

    let map f action = 
        let newAction environment = 
            let x = run environment action
            f x
        Reader newAction

    let retn x =
        let newAction environment =
            x
        Reader newAction

    let apply fAction xAction =
        let newAction environment = 
            let f = run environment fAction
            let x = run environment xAction
            f x
        Reader newAction

    let bind f xAction = 
        let newAction environment = 
            let x = run environment xAction
            run environment (f x)
        Reader newAction

    let execute action = 
        use environment = new Environment()
        environment.Open()
        let result = run environment action
        environment.Close()
        result
