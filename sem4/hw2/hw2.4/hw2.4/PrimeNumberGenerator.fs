module PrimeNumberGenerator

/// Generates an infinite sequence of prime numbers.
let generatePrimeNumbers =
    let isPrime n =
        match n with
        | _ when n <= 3 -> n > 1
        | _ -> 
            let rec isPrimeRecursive n i =
                if i * i <= n then
                    if n % i = 0 then
                        false
                    else
                        isPrimeRecursive n (i + 1)
                else
                    true
            isPrimeRecursive n 2
    Seq.initInfinite (fun n -> n + 2) |> Seq.filter isPrime