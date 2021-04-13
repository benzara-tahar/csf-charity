import { IotProvider } from '@shared';
import { DistanceUnits } from 'src/app/routes/tenant/subscriptions/models/distanceUnits';

export class EmbededDatabase {
  public static readonly supportedIoTProviders: IotProvider[] = [
    {
      name: '2hire',
      providerVersion: 1,
      description: '2hire IoT provider',
    },
    {
      name: 'Vox',
      providerVersion: 1,
      description: 'Vox IoT provider',
    },
    {
      name: 'Mock',
      providerVersion: 1,
      description: 'Mock IoT provider',
    },
  ];

  public static readonly roles: any[] = [
    {
      name: 'Admin',
    },
    {
      name: 'Tenant',
    },
    {
      name: 'User',
    },
  ];

  public static readonly adminRole = 'Admin';
  public static readonly tenantAdminRole = 'Tenant';
  public static readonly tenantUserRole = 'User';

  public static readonly distanceUnits: any[] = [
    {
      name: 'unspecified',
      value: DistanceUnits.Unspecified,
    },
    {
      name: 'kilometers',
      value: DistanceUnits.Kilometers,
    },
    {
      name: 'miles',
      value: DistanceUnits.Miles,
    },
  ];

  public static readonly supportedPermissions: any[] = [
    'query_*',
    'cmd_*',
    'query_all',
    'query_by_id',
    'query_by_lp',
    "query_vehicle_models",
    'cmd_unlock_trunk_if_closed',
    'cmd_unlock_trunk',
    'cmd_unlock_trunk_and_doors',
    'cmd_unlock_door',
    'cmd_trunk_status',
    'cmd_setup',
    'cmd_unlink',
    'cmd_profile',
    'cmd_extract'
  ];

}
