namespace FunctionalPatterns

open Download

module main =

    let goodSites = [
        "http://adobe.com"
        "http://baidu.org"
        "http://cisco.com"
        "http://dodo.com.au"
        "http://english.com"        
        ]

    let badSites = [
        "http://abc.com.au"
        "http://frankwang0.com/nopage"
        "http://frankwang1.com/nopage"
        //"http://frankwang2.com/nopage"
        ]

    let run sites = 
        sites
        |> largestPageSizeM
        |> Async.RunSynchronously
        |> showContentSizeResult

    [<EntryPoint>]
    let main argv =
        //let tuples = [Some (1,2);Some (3,4);None;Some (7,8)]
        //convert tuples |> printf "%A"

        //let tuples = None
        //tuples |> optionSequenceTuple |> printf "%A"

        //System.Uri ("http://google.com")
        //|> getUriContentSize
        //|> Async.RunSynchronously
        //|> showContentSizeResult
    
        //System.Uri ("http://google2.com")
        //|> getUriContentSize
        //|> Async.RunSynchronously
        //|> showContentSizeResult

        ////let goodRuns() = run goodSites
        ////time 1 "largestPageSizeA_Good" goodRuns

        //let badRuns() = run badSites
        //time 1 "largestPageSizeA_Bad" badRuns
        //run goodSites

        run goodSites
        0
