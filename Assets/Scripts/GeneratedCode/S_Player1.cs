using System;
using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Mappers;
using ThinkEngine.Sensors;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    class S_Player1 : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<int> values = new List<int>();

		/*
		//Singleton
        protected static Sensor instance = null;

        internal static Sensor Instance
        {
            get
            {
                if (instance == null)
                {
					instance = new S_Player1();
                }
                return instance;
            }
        }*/

		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
            // Debug.Log("Initialize method called!");
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(int));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "Player_y(player,objectIndex(1),{0})." + Environment.NewLine;

		}

		public override void Destroy()
		{
            // Debug.Log("Destroy method called!");
			//instance = null;
		}

		public override void Update()
		{
            // Debug.Log("Update method called!");
			if(!ready)
			{
				return;
			}
			if(!invariant || first)
			{
				first = false;
				GridPositionSensorData GridPositionSensorData0 = gameObject.GetComponent<GridPositionSensorData>();
				if(GridPositionSensorData0 == null) return;
				Vector2Int Pos1 = GridPositionSensorData0.Pos;
				if(Pos1 == null) return;
				int y2 = Pos1.y;

				if (values.Count == 200)
				{
					values.RemoveAt(0);
				}
				values.Add(y2);
			}
		}

		public override string Map()
		{
            // Debug.Log("Map method called!");
			object operationResult = operation(values, specificValue, counter);
			return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));
		}
    }
}
