using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viper.Framework.Enums
{
	public enum SimulationState
	{
		/// <summary>
		/// 
		/// </summary>
		Error = -1,

		/// <summary>
		/// 
		/// </summary>
		Ready = 0,

		/// <summary>
		/// 
		/// </summary>
		Running = 1,

		/// <summary>
		/// 
		/// </summary>
		RunStepByStep = 2,

		/// <summary>
		/// 
		/// </summary>
		Paused = 3,

		/// <summary>
		/// 
		/// </summary>
		Finished = 4
	}
}