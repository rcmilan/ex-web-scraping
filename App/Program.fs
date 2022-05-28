open System
open System.Globalization
open System.Text.RegularExpressions
open FSharp.Data

let convertRevenue str = Regex("(\$|,)").Replace(str, String.Empty)
                            |> Decimal.Parse
                            |> fun number -> number / 1000000000M

let convertReleaseDate str = DateTime.ParseExact(str, "MMMM dd, yyyy", CultureInfo.InvariantCulture)

let isReleaseDate str = Regex("[a-zA-Z]+ \d{2}, \d{4}", RegexOptions.IgnoreCase).IsMatch(str)

type StarWarsSearch = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_Star_Wars_films">

let films = StarWarsSearch().Tables.``Box office performanceEdit``.Rows
                |> Seq.filter (fun r -> isReleaseDate r.``US release date - Skywalker Saga``)
                |> Seq.sortBy (fun x -> convertReleaseDate x.``US release date - Skywalker Saga``)
                |> Seq.map (fun r -> r.``Film - Skywalker Saga``, convertRevenue r.``Box office gross - Worldwide - Skywalker Saga``)
                |> Seq.toArray

films |> Seq.iter (fun elem -> elem ||> printf "%s - %f Billions \n")