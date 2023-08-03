using ThinkEngine.Sensors;
using System.Collections.Generic;
using System;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class S_PlayerBot6 : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<List<string>> values = new List<List<string>>();
		private List<bool> isIndexActive = new List<bool>();
		private List<int> indicies = new List<int>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(string));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_PlayerBot_MissingIngredientsNames(playerSensors,objectIndex("+index+"),{0},{1})." + Environment.NewLine;
			PlayerBotSensorData PlayerBotSensorData0 = gameObject.GetComponent<PlayerBotSensorData>();
			if(PlayerBotSensorData0 == null) return;
			List<string> MissingIngredientsNames1 = PlayerBotSensorData0.MissingIngredientsNames;
			if(MissingIngredientsNames1 == null) return;

			for(int i = 0; i < MissingIngredientsNames1.Count; i++)
			{
				indicies.Add((i));
				isIndexActive.Add(true);
				values.Add(new List<string>());
			}

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
				if(PlayerBotSensorData0 == null) return;
				List<string> MissingIngredientsNames1 = PlayerBotSensorData0.MissingIngredientsNames;
				if(MissingIngredientsNames1 == null) return;

				if(MissingIngredientsNames1.Count > isIndexActive.Count)
				{
					for(int i = isIndexActive.Count; i < MissingIngredientsNames1.Count; i++)
					{
						indicies.Add(i);
						isIndexActive.Add(true);
						values.Add(new List<string>());
					}
				}
				else if(MissingIngredientsNames1.Count < isIndexActive.Count)
				{
					for(int i = MissingIngredientsNames1.Count; i < isIndexActive.Count; i++) {
                        var index = isIndexActive.Count - 1;
						indicies.RemoveAt(index);
						isIndexActive.RemoveAt(index);
						values.RemoveAt(index);
					}
				}
				for(int i = 0; i < values.Count; i++)
				{
					if(MissingIngredientsNames1[i] == null && isIndexActive[i])
					{
						isIndexActive[i] = false;
					}
					else if(MissingIngredientsNames1[i] != null && !isIndexActive[i])
					{
						isIndexActive[i] = true;
					}
				}
				for(int i = 0; i < values.Count; i++)
				{
					if (values[i].Count == 200)
					{
						values[i].RemoveAt(0);
					}
					values[i].Add(MissingIngredientsNames1[indicies[i]]);
				}
			}
		}

		public override string Map()
		{
			string mapping = string.Empty;
			for(int i = 0; i < values.Count; i++)
			{
				if(!isIndexActive[i]) continue;
				object operationResult = operation(values[i], specificValue, counter);
				mapping = string.Concat(mapping, string.Format(mappingTemplate, indicies[i], BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult)));
			}
			return mapping;
		}
    }
}