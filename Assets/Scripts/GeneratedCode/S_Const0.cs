using System;
using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Mappers;
using ThinkEngine.Sensors;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    class S_Const0 : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<List<string>> values = new List<List<string>>();
		private List<bool> isIndexActive = new List<bool>();
		private List<int> indicies = new List<int>();

		/*
		//Singleton
        protected static Sensor instance = null;

        internal static Sensor Instance
        {
            get
            {
                if (instance == null)
                {
					instance = new S_Const0();
                }
                return instance;
            }
        }*/

		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
            // Debug.Log("Initialize method called!");
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(string));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_Const_KitchenObjectsNames(constantSensors,objectIndex(2),{0},{1})." + Environment.NewLine;			ConstantsSensorData ConstantsSensorData0 = gameObject.GetComponent<ConstantsSensorData>();
			if(ConstantsSensorData0 == null) return;
			List<string> KitchenObjectsNames1 = ConstantsSensorData0.KitchenObjectsNames;
			if(KitchenObjectsNames1 == null) return;

			for(int i = 0; i < KitchenObjectsNames1.Count; i++)
			{
				indicies.Add((i));
				isIndexActive.Add(true);
				values.Add(new List<string>());
			}

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
				ConstantsSensorData ConstantsSensorData0 = gameObject.GetComponent<ConstantsSensorData>();
				if(ConstantsSensorData0 == null) return;
				List<string> KitchenObjectsNames1 = ConstantsSensorData0.KitchenObjectsNames;
				if(KitchenObjectsNames1 == null) return;

				if(KitchenObjectsNames1.Count > isIndexActive.Count)
				{
					for(int i = isIndexActive.Count; i < KitchenObjectsNames1.Count; i++)
					{
						indicies.Add(i);
						isIndexActive.Add(true);
						values.Add(new List<string>());
					}
				}
				else if(KitchenObjectsNames1.Count < isIndexActive.Count)
				{
					for(int i = KitchenObjectsNames1.Count; i < isIndexActive.Count; i++)
					{
						indicies.RemoveAt(isIndexActive.Count - 1);
						isIndexActive.RemoveAt(isIndexActive.Count - 1);
						values.RemoveAt(isIndexActive.Count - 1);
					}
				}
				for(int i = 0; i < values.Count; i++)
				{
					if(KitchenObjectsNames1[i] == null && isIndexActive[i])
					{
						isIndexActive[i] = false;
					}
					else if(KitchenObjectsNames1[i] != null && !isIndexActive[i])
					{
						isIndexActive[i] = true;
					}
				}				for(int i = 0; i < values.Count; i++)
				{
					if (values[i].Count == 200)
					{
						values[i].RemoveAt(0);
					}
					values[i].Add(KitchenObjectsNames1[indicies[i]]);
				}
			}
		}

		public override string Map()
		{
            // Debug.Log("Map method called!");
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
