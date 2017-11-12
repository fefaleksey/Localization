var __interpretation_started_timestamp__;
var pi = 3.141592653589793
var g=brick.gyroscope()
g.calibrate(5)
function yaw (){return g.read()[6]}
var yaw0=0, n=2,k=4

var map=
[
	[[3,1,1,1,0],[2,0,0,1,1],[3,0,1,1,1]],
	[[2,0,1,1,0],[1,1,0,0,0],[1,0,0,0,1]],
	[[3,1,1,0,1],[3,1,1,1,0],[2,1,0,0,1]]
]
//splice VS delete ?
//sparse array javascript
var hypothesis = []

const Height = 3
const Width = 3
const Down = 1
const Left = 2
const Up = 3
const Right = 4

//var 	x = [], y = []
var	lengthArr=0

// robot fields
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
var sensors = [[1,1,0,1]]
var sensorsQuality = 1
//var InitialDirection = 3
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


var main = function()
{
	__interpretation_started_timestamp__ = Date.now()
	/*
	brick.motor(M3).setPower(10);
	brick.motor(M4).setPower(-10);
	*/
	
	var g=brick.gyroscope()
	g.calibrate(5)
	function yaw (){return g.read()[6]}
	
	print(hypothesis.length)
	StartInit()
	
	HypothesisFilterInStart()
	printHypothesis()
	
	sensors[0][0] = 0
	sensors[0][1] = 1
	sensors[0][2] = 1
	sensors[0][3] = 0
	print("123")
	HypothesisFilter(Up)
	printHypothesis()
	sensors[0][0] = 0
	sensors[0][1] = 0
	sensors[0][2] = 1
	sensors[0][3] = 0
	HypothesisFilter(Right)
	printHypothesis()
	
	/*
	while(true)
	{
		//var yaw = function(){return g.read()[6];}
		//!!!!!!!!!!!!!!!!!!!
		//https://en.wikipedia.org/wiki/Event-driven_programming
		//https://en.wikipedia.org/wiki/Subsumption_architecture
		//!!!!!!!!!!!!!!!!!!!
		print(yaw());
		print(brick.sensor(A1).read());
		script.wait(100);
	}
	*/
	return
}

function Localization(){
	sensorsRead()
	HypothesisFilterInStart()
	//var value = GetSensorsValue()
	var step = 0
	//motion(/* table[value][0] */) // следующие показания сенсоров считываем во время движения
	while(hypothesis.length>1){
		var value = GetSensorsValue()
		var direction = table[value][step]
		motion(/* direction */)
		HypothesisFilter(direction)
		step++
	}
}

function HypothesisFilterInStart()
{
	print("HypothesisFilterInStart " + hypothesis.length)
	for (var i = 0; i < hypothesis.length; i++)
	{
		var x = hypothesis[i][0]
		var y = hypothesis[i][1]
		var direction = hypothesis[i][2]
		print(CheckWalls(x,y,direction))
		print(hypothesis[i])
		if (!CheckWalls(x, y, direction))
		{
			hypothesis.splice(i,1)
			i--
			print("hypothesis.splice " + hypothesis.length)
		}
	}
}

//Сделать эти 2 функции и протестить алгоритм
function HypothesisFilter(newDirection)
{
	for (var i = 0; i < hypothesis.length; ++i)
	{
		var newDir = GetNewAbsoluteDirection(hypothesis[i][2], newDirection)
		print(i + " " + newDir + " ")
		var fl = true
		switch (newDir)
		{
			case Down:
			{
				var x = hypothesis[i][0]
				var y = hypothesis[i][1]

				if (x + 1 < Height /*&& map[x + 1, y, 0] == quantity*/)
				{
					if (map[x][y][Down] == 0 && CheckWalls(x + 1, y, Down))
					{
						hypothesis[i][0]++
						fl = false
					}
				}
				break
			}
			case Left:
			{
				var x = hypothesis[i][0]
				var y = hypothesis[i][1]

				if (y > 0 /*&& map[x, y - 1, 0] == quantity*/)
				{
					if (map[x][y][Left] == 0 && CheckWalls(x, y - 1, Left))
					{
						hypothesis[i][1]--
						hypothesis[i][2] = Left
						fl = false
					}
				}
				break
			}
			case Up:
			{
				var x = hypothesis[i][0]
				var y = hypothesis[i][1]

				if (x > 0 /*&& Map[x - 1, y, 0] == quantity*/)
				{
					if (map[x][y][Up] == 0 && CheckWalls(x - 1, y, Up))
					{
						hypothesis[i][0]--
						fl = false
					}
				}
				break
			}
			case Right:
			{
				var x = hypothesis[i][0]
				var y = hypothesis[i][1]
				print(x + " " + y + " " + CheckWalls(x, y + 1, Right) + " " + map[x][y][Right])
				if (y + 1 < Width /*&& map[x, y + 1, 0] == quantity*/)
				{
					if (map[x][y][Right] == 0 && CheckWalls(x, y + 1, Right))
					{
						hypothesis[i][1]++
						hypothesis[i][2] = Right
						fl = false
					}
				}
				break
			}
		}

		if (fl)
		{
			hypothesis.splice(i,1)
			i--
		}
	}
}

function StartInit()
{
	var currentHypothesis = [0,0,1]
	for(i = 0; i < Height; i++)
	{
		for(j = 0; j < Width; j++)
		{
			for(k = 1; k <= 4; k++)
			{
				var currentHypothesis = [i,j,k]
				hypothesis.push(currentHypothesis)
			}
		}
	}
	printHypothesis()
}

function HypothesisInit()
{
	var index = 0;
	for(i = 0; i < Height; i++)
	{
		for(j = 0; j < Width; j++)
		{
			for(k = 1; k <= 4; k++)
			{
				hypothesis[index][0] = i
				hypothesis[index][1] = j
				hypothesis[index][2] = k
				index++
			}
		}
	}
}

function GetNewAbsoluteDirection(currentDirection, newDirection)
{
	switch (newDirection)
	{
		case Down:
		{
			if(currentDirection > 2) return currentDirection - 2
			return currentDirection + 2
		}
		case Left:
		{
			if (currentDirection > 1) return currentDirection - 1
			return 4
		}
		case Up:
		{
			return currentDirection
		}
		case Right:
		{
			if (currentDirection < Right) return currentDirection + 1
			return Down
		}
	}
}

function printHypothesis()
{
	for(i = 0; i < hypothesis.length; i++)
	{
		print(i + "   " + hypothesis[i])
	}
}

function sensorsRead(){
	
}

function motion(direction){
	
}

/*
function printMap()
{
	var i,j;
	for(i = 0; i <= n; i++) { 
			print(map[i][0] + "  "+ map[i][1] + "  "+map[i][2])
		}
}
*/

function CheckWalls(x, y, direction)
{
	var startX = x
	var startY = y
	//Robot.Sensors = _sensors;
	/*
	if (robot.InitialDirection == 1)
	{
		if (direction > 2) direction -= 2;
		else direction += 2;
	}
	*/
	var i = 0
	//Down
	while (i < sensorsQuality && x + 1 <= Height)
	{
		var j = GetIndex(direction, Down)
		if (sensors[i][j] != map[x][y][Down]) return false
		if (sensors[i][j] == 1) break
		i++
		x++
	}
	x = startX
	i = 0
	//Left
	while (i < sensorsQuality && y >= 0)
	{
		var j = GetIndex(direction, Left)
		if (sensors[i][j] != map[x][y][Left]) return false
		if (sensors[i][j] == 1) break
		i++
		y--
	}
	y = startY
	i = 0
	//Up
	while (i < sensorsQuality && x >= 0)
	{
		var j = GetIndex(direction, Up)
		if (sensors[i][j] != map[x][y][Up]) return false
		if (sensors[i][j] == 1) break
		i++
		x--
	}
	x = startX
	i = 0
	//Right
	while (i < sensorsQuality && y + 1 <= Width)
	{
		var j = GetIndex(direction, Right)
		if (sensors[i][j] != map[x][y][Right]) return false
		if (sensors[i][j] == 1) break
		i++
		y++
	}
	return true
}

function GetIndex(direction, newDirection)
{
	if (newDirection == direction + 2 || newDirection == direction - 2)
		return 0
	if (newDirection == direction)
		return 2
	if (newDirection == direction + 1 || newDirection == direction - 3)
		return 3
	return 1
}
