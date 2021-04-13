export class IoTAccessAccount {
  id: string;
  tenantId: string;
  tenant: any;
  userName: string;
  password: string;
  passwordHash: string;
  permissions: string[];
  validTo: string;
  updatedAtUtc: string | null;
}
