using System;
using System.Collections.Generic;
using CookedUp.ThinkEngine.Models;
using CookedUp.ThinkEngine.Sensors;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_PlayerBot_CurrentRecipeRequest_ID : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<int> values = new List<int>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(int));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_PlayerBot_CurrentRecipeRequest_ID(player,objectIndex("+index+"),{0})." + Environment.NewLine;

		}

		public override void Destroy()
		{
		}

		public override void Update()
		{
			if(!ready)
			{
				return;
			}
			if(!invariant || first)
			{
				first = false;
				PlayerBotSensorData PlayerBotSensorData0 = gameObject.GetComponent<PlayerBotSensorData>();
				RecipeRequestASP CurrentRecipeRequestASP1 = PlayerBotSensorData0.CurrentRecipeRequestASP;
                if (CurrentRecipeRequestASP1 == null) {
                    values.Clear();
                    return;
                }
				int ID2 = CurrentRecipeRequestASP1.ID;
				if (values.Count == 200)
				{
						values.RemoveAt(0);
				}
				values.Add(ID2);
			}
		}

		public override string Map()
		{
			object operationResult = operation(values, specificValue, counter);
			if(operationResult != null)
			{
				return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));
			}
			else
			{
				return "";
			}
		}
    }
}
