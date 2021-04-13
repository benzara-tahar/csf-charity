export interface Tenant {
  id: string;
  name: string;
  supportedIoTProviders: IotProvider[];
  registrationDate: Date;
}

export interface IotProvider {
  name: string;
  providerVersion: number;
  description: string;
}
