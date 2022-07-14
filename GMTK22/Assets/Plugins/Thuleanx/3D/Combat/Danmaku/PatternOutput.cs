using UnityEngine;
using XNode;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using NaughtyAttributes;

namespace Thuleanx3D.Combat.Danmaku {
	public class PatternOutput: Node {
		[Input(connectionType = ConnectionType.Override)] public PatternComposite Compose;

		public PatternComposite GetPattern() => GetInputValue<PatternComposite>("Compose");
	}
}