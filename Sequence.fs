namespace FunctionalPatterns

module Sequence =

    let tupleReturn x = (x, x)
    let tupleApply (f, g) (x, y) = (f x, g y)

    let listSequenceTuple list = 
        let (<*>) = tupleApply
        let retn = tupleReturn

        let cons  head tail = head :: tail
    
        let initState = retn []
        let folder head tail = retn cons <*> head <*> tail

        List.foldBack folder list initState

    let optionSequenceTuple opt =
        let (<*>) = tupleApply
        let retn = tupleReturn

        let initState = retn None
        let folder x _ = (retn Some) <*> x

        Option.foldBack folder opt initState

    let convert input =
        input
        |> List.map optionSequenceTuple
        |> listSequenceTuple