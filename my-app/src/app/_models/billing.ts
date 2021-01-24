export class Item {
	public id: number;
	public name: string;
	public desc: string;
	public price: number;
}

export class BillingItem extends Item {
	public quantity: number;
}

export class User {
	public id: number;
	public firstname: string;
	public lastname: string;
	public username: string;
	public password: string;
}