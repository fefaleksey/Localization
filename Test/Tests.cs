using Xunit;
using Localization;

namespace Test
{
	public class TestsRobot
	{
		[Fact]
		public void TestGetSensorsValue()
		{
			var robot = new Robot();
			var map = new HandlingHypotheses();
			robot.InitialDirection = 3;
			robot.RSensors.Read(0, 0, 1, robot, map);
			var value = robot.RSensors.GetSensorsValue(robot);
			Assert.True(value == 36896);
			robot.RSensors.Read(0, 6, 1, robot, map);
			value = robot.RSensors.GetSensorsValue(robot);
			Assert.True(value == 61440);
		}
	}
}