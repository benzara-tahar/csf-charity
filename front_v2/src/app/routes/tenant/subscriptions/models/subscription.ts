import { DistanceUnits } from './distanceUnits';

export class Subscription {
  id: string;
  tenantId: string;
  tenant: any;
  name: string;
  webhookUri: string;
  secret: string;
  isActive: boolean;
  batchInterval: number;
  maxAttemptBeforeRejection: number;
  webhooks: string[];
  preferredDistanceUnit: DistanceUnits;
}

