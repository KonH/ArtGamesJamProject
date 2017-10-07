public struct Resource_New {
	public ResourceHolder Holder;

	public Resource_New(ResourceHolder holder) {
		Holder = holder;
	}
}

public struct Resource_Update {
	public ResourceHolder Holder;

	public Resource_Update(ResourceHolder holder) {
		Holder = holder;
	}
}

public struct Turn_New {
	public int Number;

	public Turn_New(int number) {
		Number = number;
	}
}

public struct Region_Update {
	public Region Region;

	public Region_Update(Region region) {
		Region = region;
	}
}

public struct Game_End {
	public Resource Resource;

	public Game_End(Resource resource) {
		Resource = resource;
	}
}

public struct User_Restart {}