import { Country } from './country';
import { ItemType } from './item-type';

export class Sale {
    public id: number;
    constructor(public country: Country, public itemType: ItemType, public salesChanel: string,
        public orderPriority: string, public orderId: number, public orderDate: Date,
        public shipDate: Date, public unitsSold: number, public unitPrice: number, public unitCost: number) { }
}