using System;

	[Serializable]
public class ResourceHolder {
		public Resource Resource;
		public int Count;
		public int Change;

		public ResourceHolder(Resource resource, int count) {
			Resource = resource;
			Count = count;
		}
	}
