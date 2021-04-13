export interface WebhookEvent {
  id: string;
  name: string;
  subscriptionName: string;
  addedAtUtc: Date;
  enqueuedAtUtc: Date;
  lastDeliverAttempt?: Date;
  retryCount: number;
  status: string;
  url: string;
  statusCode: number;
  response: string;
  payload: string;
  version: string;
  deviceId: string;
}

export enum WebhookEventStatus {
  Pending,
  DeliverySucceeded,
  DeliveryFailed,
  Rejected,
}
