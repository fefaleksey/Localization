// Learn more about F# at http://fsharp.org

namespace Localization

open System
open System.Collections
open System.Collections.Generic

type Motion() = class
    let Down = 1
    let Left = 2
    let Up = 3
    let Right = 4

    member this.ToRightDir direction = 
        if direction < 4 then direction + 1
        else 1


    //Возвращает направление, если поворачиваем в соотв. сторону
    member this.ToLeftDir direction =
        if direction > 1 then direction - 1
        else 4

    member this.ToDownDir direction (beginWay : bool) =
        if beginWay then
            if direction > 2 then direction - 2
            else direction + 2
        else direction

    // текущее направление в абсолютных коорд/направление движения(куда едем?)
    member this.GetNewDir (currentDirection : int) (newDirection : int) (beginWay : bool) =
        if newDirection = Up then currentDirection 
        elif newDirection = Right then this.ToRightDir <| currentDirection
        elif newDirection = Left then this.ToLeftDir currentDirection
        elif newDirection = Down then this.ToDownDir currentDirection beginWay
        else
            Console.WriteLine("Motion.GetNewDir - Bag")
            -1
    end

type Robot() = class
    member val public Sensors : int[,] = Array2D.init RobotSensors.QualitySensors 4 (fun i j -> 0) //with get,set
    member val RSensors : RobotSensors = new RobotSensors()
    member val BeginWay : bool = true
    member val public InitialDirection : int = 1 with get,set
    end
and RobotSensors() = class
    let Down = 0
    let Left = 1
    let Up = 2
    let Right = 3
    let IDown = 1
    let ILeft = 2
    let IUp = 3
    let IRight = 4
    static member val QualitySensors = 2 // количество клеток, на которых сенсоры работают адекватно

    // Only for method SensorRead
    member this.GetIndex direction newDirection =
        if newDirection = direction + 2 || newDirection = direction - 2 then 0
        elif newDirection = direction then 2
        elif newDirection = direction + 1 || newDirection = direction - 3 then 3
        else 1

    /// <param name="robot"> robot</param>
    /// <returns> hash key </returns>
    member this.GetSensorsValue (robot : Robot) =
        let mutable value = 0
        let mutable n = RobotSensors.QualitySensors * 4
        for i in 0..(RobotSensors.QualitySensors - 1) do
            for j in 0 ..3 do
                n <- n - 1
                value <- value + (int (Math.Pow((float 2), (float n)))) * robot.Sensors.[i,j]
        value

    member this.Read (x : int ) (y : int) (direction : int) (robot : Robot) (handlingHypotheses : HandlingHypotheses) =
        let mutable i = 0
        let mutable x = x
        let mutable y = y
        let mutable startX = x
        let mutable startY = y
        for a in 0..(RobotSensors.QualitySensors - 1) do
            for b in 0..3 do
                robot.Sensors.[a, b] <- 0

        //Down
        while i < RobotSensors.QualitySensors && x + 1 <= HandlingHypotheses.Height do
            let j = this.GetIndex direction IDown
            robot.Sensors.[i, j] <- handlingHypotheses.Map.[x].[y].[IDown]
            if robot.Sensors.[i, j] <> 1 then //break
                i <- i + 1
                x <- x + 1
            else 
                i <- RobotSensors.QualitySensors
        
        x <- startX
        y <- startY
        i <- 0
        
        //Left
        while i < RobotSensors.QualitySensors && y >= 0 do
            let j = this.GetIndex direction ILeft
            robot.Sensors.[i, j] <- handlingHypotheses.Map.[x].[y].[ILeft]
            if robot.Sensors.[i, j] <> 1 then
                i <- i + 1
                y <- y - 1
            else i <- RobotSensors.QualitySensors

        x <- startX
        y <- startY
        i <- 0    
        
        //Up
        while i < RobotSensors.QualitySensors&& x >= 0 do
            let j = this.GetIndex direction IUp
            robot.Sensors.[i, j] <- handlingHypotheses.Map.[x].[y].[IUp]
            if (robot.Sensors.[i, j] <> 1) then
                i <- i + 1
                x <- x - 1
            else i <- RobotSensors.QualitySensors
    
        x <- startX
        y <- startY
        i <- 0
        //Right
        while i < RobotSensors.QualitySensors && y + 1 <= HandlingHypotheses.Width do
            let j = this.GetIndex direction IRight
            robot.Sensors.[i, j] <- handlingHypotheses.Map.[x].[y].[IRight]
            if robot.Sensors.[i, j] <> 1 then
                i <- i + 1
                y <- y + 1
            else i <- RobotSensors.QualitySensors
        
        if robot.InitialDirection = 1 then
            for j in 0..(RobotSensors.QualitySensors - 1) do
                let value1 = robot.Sensors.[j, Down]
                let value2 = robot.Sensors.[j, Left]
                robot.Sensors.[j, Down] <- robot.Sensors.[j, Up]
                robot.Sensors.[j, Left] <- robot.Sensors.[j, Right]
                robot.Sensors.[j, Up] <- value1
                robot.Sensors.[j, Right] <- value2
    end

and Way() = class
    let mutable _i = 0
    let mutable _forbiddenDirection = 3
    let Length = 64
    member val public CurentWay : List<int> = List<int> [] with get,set // = (new List<int>())
    member val public BeginWay : bool = true with get,set
    
    member this.CurentWayInit _ =
        this.CurentWay.Clear ()
        this.CurentWay <- List<int> [1]
        _forbiddenDirection <- 3
        _i <- 0

    //localiz==true <=> мы локализовались
    member this.NextDirection (wayExist : bool) (localiz : bool) (robot : Robot) : int =
        let mutable result = -1 //!!
        let rec getNext (wayExist : bool) (localiz : bool) (robot : Robot) : unit =
            if this.CurentWay.Count = 0 then result <- 0
            else
                if localiz then
                    if this.CurentWay.[_i] = 4 then
                        this.CurentWay.RemoveAt _i
                        _i <- _i - 1
                        //this.NextDirection false false robot
                        result <- this.NextDirection false false robot
                    else
                        this.CurentWay.[_i] <- this.CurentWay.[_i] + 1

                        if this.CurentWay.Count = 1 && this.CurentWay.[0] = 2 then
                            //запрещенное направление мы поменяем только 1 раз!
                            _forbiddenDirection <- 1
                            robot.InitialDirection <- 3
                        else ()
                        
                        if this.CurentWay.[_i] = _forbiddenDirection then 
                            this.CurentWay.[_i] <- this.CurentWay.[_i] + 1
                        else ()
                    result <- this.CurentWay.[_i]
                else ()
                
                if wayExist then
                    if this.CurentWay.Count = Length then
                        result <- this.NextDirection false false robot
                    else ()
                
                    if _forbiddenDirection = 1 then
                        this.CurentWay.Add 2
                        _i <- _i + 1
                        result <- 2
                    else
                        this.CurentWay.Add 1
                        _i <- _i + 1
                        result <- 1
                else 
                    if this.CurentWay.[_i] = 4 then
                        this.CurentWay.RemoveAt _i
                        _i <- _i - 1
                        result <- this.NextDirection false false robot
                    else 
                        this.CurentWay.[_i] <- this.CurentWay.[_i] + 1

                        if this.CurentWay.Count = 1 && this.CurentWay.[0] = 2 then
                            _forbiddenDirection <- 1
                            robot.InitialDirection <- 3
                        else ()

                        if this.CurentWay.[_i] = _forbiddenDirection then
                            this.CurentWay.[_i] <- this.CurentWay.[_i] + 1
                        else ()

                        result <- this.CurentWay.[_i]
        
        getNext wayExist localiz robot
        result
    end

    
and HandlingAllCases() = class
    let mutable _wayExist = true
    let mutable _localiz = false

    member this.CopyWay((way : Way), (To : List<int> byref)) =
        To.Clear()
        for i in 0..(way.CurentWay.Count - 1) do
            To.Add way.CurentWay.[i]
            
            
    member this.Handing  (handlingHypotheses : HandlingHypotheses) (way : Way) =  
        let copyHypothesis = new List<List<int>>()
        handlingHypotheses.HypothesisInit()
        let ways = new List<int>()
        
        for i in 0..(handlingHypotheses.Hypothesis.Count - 1) do
            copyHypothesis.Add (new List<int>())
        
        let mutable fl = 1
        handlingHypotheses.Copy_Lists((ref copyHypothesis), handlingHypotheses.Hypothesis)
        let mutable n = handlingHypotheses.Hypothesis.[0].Count
        let motion = new Motion()
        let robot = new Robot() 
        ()
        
        for i in 0..(n-1) do
            way.CurentWayInit ()
            while fl <> 0 do
                this.CopyWay(way, (ref ways))
                robot.InitialDirection <- 3
                robot.RSensors.Read handlingHypotheses.Hypothesis.[0].[i] handlingHypotheses.Hypothesis.[1].[i] handlingHypotheses.Hypothesis.[2].[i] robot handlingHypotheses
                
                this.HandingHypothesis i ways handlingHypotheses motion way robot
                fl <- way.NextDirection _wayExist _localiz robot
                handlingHypotheses.Hypothesis.[0].Clear();
                handlingHypotheses.Hypothesis.[1].Clear();
                handlingHypotheses.Hypothesis.[2].Clear();
                handlingHypotheses.Copy_Lists((ref handlingHypotheses.Hypothesis), copyHypothesis)
                _wayExist <- true
                _localiz <- false
            fl <- 1

    //number -- номер гипотезы
    member this.HandingHypothesis (number : int) (ways : List<int>) (handlingHypotheses : HandlingHypotheses) (motion : Motion) (way : Way) (robot : Robot) =
        let Down = 1
        let Left = 2
        let Up = 3
        let Right = 4

        let mutable x = handlingHypotheses.Hypothesis.[0].[number]
        let mutable y = handlingHypotheses.Hypothesis.[1].[number]
        let direction = handlingHypotheses.Hypothesis.[2].[number]
        let xStart = x
        let yStart = y
        let mutable newDir = handlingHypotheses.Hypothesis.[2].[number]
        way.BeginWay <- true;
        handlingHypotheses.HypothesisFilter robot
        for k in 0..(ways.Count - 1) do
            let mutable fl = true
            if k = 1 then way.BeginWay <- false else ()

            newDir <- motion.GetNewDir newDir ways.[k] way.BeginWay
            if newDir = Down then
                if (x + 1) < HandlingHypotheses.Height && handlingHypotheses.Map.[x].[y].[Down] = 0 then
                    fl <- false;
                    x <- x + 1
                    robot.RSensors.Read x y Down robot handlingHypotheses
            if newDir = Left then
                if y > 0 && handlingHypotheses.Map.[x].[y].[Left] = 0 then
                    fl <- false
                    y <- y - 1
                    robot.RSensors.Read x y Left robot handlingHypotheses
            if newDir = Up then
                if x > 0 && handlingHypotheses.Map.[x].[y].[Up] = 0 then
                    fl <- false
                    x <- x - 1
                    robot.RSensors.Read x y Up robot handlingHypotheses
            if newDir = Right then
                if (y + 1) < HandlingHypotheses.Width && handlingHypotheses.Map.[x].[y].[Right] = 0 then
                    fl <- false
                    y <- y + 1
                    robot.RSensors.Read x y Right robot handlingHypotheses
            
            if fl then
                handlingHypotheses.Sensors.[0] <- -1
                handlingHypotheses.Sensors.[1] <- -1
                handlingHypotheses.Sensors.[2] <- -1
                handlingHypotheses.Sensors.[3] <- -1
                _wayExist <- false
                //return
            else
                handlingHypotheses.Hypothesis3 ways.[k] way.BeginWay motion robot
                if handlingHypotheses.Hypothesis.[0].Count = 1 then
                    _localiz <- true
                    let n = handlingHypotheses.BestWays.Count;
                    handlingHypotheses.BestWays.Add (new List<int>())
                    handlingHypotheses.BestWays.[n].Add xStart
                    handlingHypotheses.BestWays.[n].Add yStart
                    handlingHypotheses.BestWays.[n].Add direction
                    for l in 0..k do
                        handlingHypotheses.BestWays.[n].Add ways.[l]
                        //	Console.Write(best_ways[n][l] + ",");
                    //Console.WriteLine(" quantity of steps: " + k);
                    //return;
                elif handlingHypotheses.Hypothesis.[0].Count = 0 then
                    _wayExist <- false
                    //return
                else ()
    end

and FinalWays() = class
    //Ways: координаты, путь, 8888888, координаты после локализации, 8888888, время
    
    member val public  Ways : List<List<int>> = new List<List<int>>() with get,set
    member val public FinalList : List<List<int>> = new List<List<int>>() with get,set
    
    member this.Init _ =
        let map = new HandlingHypotheses ()
        map.HypothesisInit ()
        for i in 0..(map.Hypothesis.[0].Count - 1) do
            this.FinalList.Add (new List<int>())
            this.FinalList.[i].Add map.Hypothesis.[0].[i]
            this.FinalList.[i].Add map.Hypothesis.[1].[i]
            this.FinalList.[i].Add map.Hypothesis.[2].[i]
    
    
    member this.SetFinalList _  =  
        for i in 0..(this.FinalList.Count - 1) do
            let j = this.Ways.[i].Count - 1
            this.FinalList.[i].Add this.Ways.[i].[j]
            
      
    (*
    member this.PrintResult _ =
        for i in 0..(this.FinalList.Count - 1) do 
            Console.Write(i + ". ")
            for j in 0..(this.FinalList.[i].Count - 1) do            
                Console.Write(this.FinalList.[i].[j] + " ")
                
            Console.WriteLine();
    *)
    end

and HandlingHypotheses() = class//as this =

    let Down = 1
    let Left = 2
    let Up = 3
    let Right = 4
    
    member val public Map : int[][][] = [|
        [|[|2; 0; 1; 1; 0|]; [|2; 1; 0; 1; 0|]; [|1; 0; 0; 1; 0|]; [|2; 1; 0; 1; 0|];
        [|2; 1; 0; 1; 0|]; [|2; 0; 0; 1; 1|]; [|4; 1; 1; 1; 1|]; [|3; 0; 1; 1; 1|] |];
    
        [|[|2; 0; 1; 0; 1|]; [|4; 1; 1; 1; 1|]; [|2; 0; 1; 0; 1|]; [|3; 0; 1; 1; 1|];
        [|4; 1; 1; 1; 1|]; [|1; 0; 1; 0; 0|]; [|2; 1; 0; 1; 0|]; [|2; 1; 0; 0; 1|]|];
    
        [|[|2; 1; 1; 0; 0|]; [|2; 1; 0; 1; 0|]; [|1; 0; 0; 0; 1|]; [|1; 0; 1; 0; 0|];
        [|2; 1; 0; 1; 0|]; [|1; 0; 0; 0; 1|]; [|4; 1; 1; 1; 1|]; [|3; 0; 1; 1; 1|]|];

        [|[|3; 0; 1; 1; 1|]; [|4; 1; 1; 1; 1|]; [|2; 0; 1; 0; 1|]; [|3; 1; 1; 0; 1|];
        [|4; 1; 1; 1; 1|]; [|1; 0; 1; 0; 0|]; [|2; 1; 0; 1; 0|]; [|1; 0; 0; 0; 1|]|];
    
        [|[|1; 0; 1; 0; 0|]; [|2; 1; 0; 1; 0|]; [|0; 0; 0; 0; 0|]; [|2; 1; 0; 1; 0|];
        [|1; 0; 0; 1; 0|]; [|1; 0; 0; 0; 1|]; [|4; 1; 1; 1; 1|]; [|3; 1; 1; 0; 1|]|];

        [|[|3; 1; 1; 0; 1|]; [|4; 1; 1; 1; 1|]; [|2; 0; 1; 0; 1|]; [|4; 1; 1; 1; 1|];
        [|2; 1; 1; 0; 0|]; [|0; 0; 0; 0; 0|]; [|2; 1; 0; 1; 0|]; [|2; 0; 0; 1; 1|]|];

        [|[|2; 0; 1; 1; 0|]; [|2; 1; 0; 1; 0|]; [|0; 0; 0; 0; 0|]; [|2; 0; 0; 1; 1|];
        [|4; 1; 1; 1; 1|]; [|3; 1; 1; 0; 1|]; [|4; 1; 1; 1; 1|]; [|2; 0; 1; 0; 1|]|];

        [|[|3; 1; 1; 0; 1|]; [|4; 1; 1; 1; 1|]; [|2; 1; 1; 0; 0|]; [|1; 1; 0; 0; 0|];
        [|2; 1; 0; 1; 0|]; [|2; 1; 0; 1; 0|]; [|2; 1; 0; 1; 0|]; [|2; 1; 0; 0; 1|]|]
    |]

    member val public Sensors : int[] = [|0;0;0;0|] with get,set
    static member val public Height : int = 8
    static member val public Width : int = 8
    
    // x,y,direction
    //[<DefaultValue>] val mutable public Hypothesis : List<List<int>>
    member val public Hypothesis : List<List<int>> = new List<List<int>>() with get,set

    //way: x, y, directions
    [<DefaultValue>] val mutable BestWays : List<List<int>>
    //val mutable BestWays : List<List<int>> = {BestWays = new List<List<int>>}

    member this.StartInit _ = 
        //this.Hypothesis <- (new List<List<int>>())
        this.BestWays <- (new List<List<int>>())
        for i in 0..2 do
            this.Hypothesis.Add (new List<int>())

    member this.HypothesisInit _ =
        this.Hypothesis.Clear()
        this.StartInit()
        for i in 0..(HandlingHypotheses.Width - 1) do
            for j in 0..(HandlingHypotheses.Height - 1) do
                for k in 1..4 do
                    this.Hypothesis.[0].Add i
                    this.Hypothesis.[1].Add j
                    this.Hypothesis.[2].Add k

    member this.Copy_Lists((To : List<List<int>> byref), (from : List<List<int>>)) =
        let m = from.Count
        let n = from.[0].Count
        To.Clear()
        for i in 0..(m - 1) do
            To.Add (new List<int>())

        for i in 0..(n - 1) do
            To.[0].Add from.[0].[i]
            To.[1].Add from.[1].[i]
            To.[2].Add from.[2].[i]

    member this.ListFiltration (list : List<List<int>> byref) =
        let mutable i = 0
        while i < list.Count do
            let mutable j = i + 1
            while j < list.Count do
                let mutable fl = true
                let mutable k = 0
                while k < list.[i].Count && k < list.[j].Count do
                    if list.[i].[k] <> list.[j].[k] then fl <- false
                    k <- k + 1

                if fl then
                    if list.[i].Count = list.[j].Count then
                        list.RemoveAt j
                        j <- j - 1
                    elif list.[i].Count < list.[j].Count then
                        list.RemoveAt j
                        j<- j - 1
                    else 
                        list.RemoveAt i
                        i <- j
                else ()
                j <- j + 1
            i <- i + 1

    member this.Localization _ =
        let handingOfAllCases = new HandlingAllCases()
        let way = new Way() 
        handingOfAllCases.Handing this way
        this.ListFiltration <|ref this.BestWays
        
    member this.CheckWalls (xCord : int) (yCord : int) (direction : int) (robot : Robot) =
        let mutable startX = xCord
        let mutable startY = yCord
        let mutable x = xCord
        let mutable y = yCord
        let mutable direction = direction
        let mutable result = false
        let mutable exit = false
        let mutable stop = false
        if robot.InitialDirection = 1 then
            if direction > 2 then direction <- direction - 2
            else direction <- direction + 2
        let mutable i = 0
        //Down
        if exit = false then
            while i < RobotSensors.QualitySensors && x + 1 <= HandlingHypotheses.Height do
                let j = robot.RSensors.GetIndex direction Down
                if robot.Sensors.[i, j] <> this.Map.[x].[y].[Down] then 
                    exit <- true
                    stop <- true
                elif robot.Sensors.[i, j] = 1 then stop <- true
                elif stop = false then
                    i <- i + 1
                    x <- x + 1
                else ()

        else ()
        x <- startX
        i <- 0;
        stop <- false
        //Left
        if exit = false then
            while i < RobotSensors.QualitySensors && y >= 0 do
                let j = robot.RSensors.GetIndex direction Left
                if robot.Sensors.[i, j] <> this.Map.[x].[y].[Left] then 
                    exit <- true
                    stop <- true
                elif robot.Sensors.[i, j] = 1 then stop <- true
                elif stop = false then
                    i <- i + 1
                    y <- y - 1
                else ()
        else ()
        y <- startY
        i <- 0
        stop <- false
        //Up
        if exit = false then
            while i < RobotSensors.QualitySensors && x >= 0 do
                let j = robot.RSensors.GetIndex direction Up
                if robot.Sensors.[i, j] <> this.Map.[x].[y].[Up] then 
                    exit <- true
                    stop <- true
                elif robot.Sensors.[i, j] = 1 then stop <- true
                elif stop = false then   
                    i <- i + 1
                    x <- x - 1
                else ()
        else ()
        x <- startX
        i <- 0
        stop <- false
        //Right
        if exit = false then
            while i < RobotSensors.QualitySensors && y + 1 <= HandlingHypotheses.Width do
                let j = robot.RSensors.GetIndex direction Right
                if robot.Sensors.[i, j] <> this.Map.[x].[y].[Right] then
                    exit <- true
                    stop <- true
                elif robot.Sensors.[i, j] = 1 then stop <- true
                elif stop = false then
                    i <- i + 1
                    y <- y + 1
                else ()
        else ()
        if exit = false then result <- true else ()
        result



        
    member this.HypothesisFilter (robot : Robot) = 
        this.HypothesisInit ()
        let mutable i = 0
        while i < (this.Hypothesis.[0].Count - 1) do
            let x = this.Hypothesis.[0].[i]
            let y = this.Hypothesis.[1].[i]
            let direction = this.Hypothesis.[2].[i]
            if this.CheckWalls x y direction robot = false then
                this.Hypothesis.[0].RemoveAt i
                this.Hypothesis.[1].RemoveAt i
                this.Hypothesis.[2].RemoveAt i
                i <- i - 1
            i <- i + 1

    member this.Hypothesis3 (direction : int) (beginWay : bool) (motion : Motion) (robot : Robot) =
        let mutable i = 0
        let quantity = robot.Sensors.[0, 0] + robot.Sensors.[0, 1] + robot.Sensors.[0, 2] + robot.Sensors.[0, 3];

        while i < this.Hypothesis.[0].Count do
            let newDir = motion.GetNewDir this.Hypothesis.[2].[i] direction beginWay
            let mutable fl = true

            if newDir = Down then
                let x = this.Hypothesis.[0].[i]
                let y = this.Hypothesis.[1].[i]

                if (x + 1) < HandlingHypotheses.Height && this.Map.[x + 1].[y].[0] = quantity then
                    if this.Map.[x].[y].[Down] = 0 && this.CheckWalls (x + 1) y Down robot then
                        this.Hypothesis.[0].[i] <- this.Hypothesis.[0].[i] + 1
                        this.Hypothesis.[2].[i] <- Down
                        fl <- false
                    else ()
                else ()
            elif newDir = Left then
                let x = this.Hypothesis.[0].[i]
                let y = this.Hypothesis.[1].[i]
                if y > 0 && this.Map.[x].[y - 1].[0] = quantity then
                    if this.Map.[x].[y].[Left] = 0 && this.CheckWalls x (y - 1) Left robot then
                        this.Hypothesis.[1].[i] <- this.Hypothesis.[1].[i] - 1
                        this.Hypothesis.[2].[i] <- Left
                        fl <- false
                    else ()
                else ()
            elif newDir = Up then
                let x = this.Hypothesis.[0].[i]
                let y = this.Hypothesis.[1].[i]
                if x > 0 && this.Map.[x - 1].[y].[0] = quantity then
                    if this.Map.[x].[y].[Up] = 0 && this.CheckWalls (x - 1) y Up robot then
                        this.Hypothesis.[0].[i] <- this.Hypothesis.[0].[i] - 1
                        this.Hypothesis.[2].[i] <- Up
                        fl <- false
                    else ()
                else ()
            elif newDir = Right then
                let x = this.Hypothesis.[0].[i]
                let y = this.Hypothesis.[1].[i]
                if (y + 1) < HandlingHypotheses.Width && this.Map.[x].[y + 1].[0] = quantity then
                    if this.Map.[x].[y].[Right] = 0 && this.CheckWalls x (y + 1) Right robot then
                        this.Hypothesis.[1].[i] <- (this.Hypothesis.[1].[i] + 1)
                        this.Hypothesis.[2].[i] <- Right
                        fl <- false
                    else ()
                else ()
            else ()
            
            if fl then
                this.Hypothesis.[0].RemoveAt i
                this.Hypothesis.[1].RemoveAt i
                this.Hypothesis.[2].RemoveAt i
                i <- i - 1
            else ()
            i <- i + 1
    end
