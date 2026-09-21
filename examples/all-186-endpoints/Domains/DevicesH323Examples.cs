using ZoomMeetings.Models;

namespace ZoomMeetings.Examples.AllEndpoints.Domains;

public static class DevicesH323Examples
{
    public static async Task RunAsync(ZoomClient client, string dummyDeviceId = "dev_123456789")
    {
        Console.WriteLine("\n--- Devices & ZDM (8 ops) ---");
        await DomainRunner.RunOperationAsync("listDevices", "GET /devices", async () =>
            await client.ListDevicesAsync(pageSize: 10));
        await DomainRunner.RunOperationAsync("addDevice", "POST /devices", async () =>
            await client.CreateDeviceAsync(new AddDeviceRequest { DeviceName = "Conference Room A", MacAddress = "00:1A:2B:3C:4D:5E", Model = "Model X", SerialNumber = "SN12345", Vendor = "poly" }));
        await DomainRunner.RunOperationAsync("getDevice", "GET /devices/{deviceId}", async () =>
            await client.GetDeviceAsync(dummyDeviceId));
        await DomainRunner.RunOperationAsync("updateDevice", "PATCH /devices/{deviceId}", async () =>
            await client.UpdateDeviceAsync(dummyDeviceId, new UpdateDeviceRequest { DeviceName = "Conference Room B" }));
        await DomainRunner.RunOperationAsync("deleteDevice", "DELETE /devices/{deviceId}", async () =>
            await client.DeleteDeviceAsync(dummyDeviceId));
        await DomainRunner.RunOperationAsync("assginGroup", "PATCH /devices/{deviceId}/assign_group", async () =>
            await client.AssignDeviceGroupAsync(dummyDeviceId, "grp_123"));
        await DomainRunner.RunOperationAsync("changeDeviceAssociation", "PATCH /devices/{deviceId}/assignment", async () =>
            await client.ChangeDeviceAssociationAsync(dummyDeviceId, new ChangeDeviceAssociationRequest { RoomId = "room_123" }));
        await DomainRunner.RunOperationAsync("Getzdmgroupinfo", "GET /devices/groups", async () =>
            await client.ListDeviceGroupsAsync(pageSize: 10));

        Console.WriteLine("\n--- Zoom Phone Appliance (ZPA) (5 ops) ---");
        await DomainRunner.RunOperationAsync("Assigndevicetoauser/commonarea", "POST /devices/zpa/assignment", async () =>
            await client.AssignZpaDeviceAsync(new ZpaAssignmentRequest { MacAddress = "00:1A:2B:3C:4D:5E", Vendor = "poly" }));
        await DomainRunner.RunOperationAsync("GetZpaDeviceListProfileSettingOfaUser", "GET /devices/zpa/settings", async () =>
            await client.GetZpaUserSettingsAsync("me"));
        await DomainRunner.RunOperationAsync("UpgradeZpas/app", "POST /devices/zpa/upgrade", async () =>
            await client.UpgradeZpaAsync(new ZpaUpgradeRequest { ZdmGroupId = "grp_123" }));
        await DomainRunner.RunOperationAsync("DeleteZpaDeviceByVendorAndMacAddress", "DELETE /devices/zpa/vendors/{vendor}/mac_addresses/{macAddress}", async () =>
            await client.DeleteZpaDeviceAsync("poly", "00:1A:2B:3C:4D:5E"));
        await DomainRunner.RunOperationAsync("GetZpaVersioninfo", "GET /devices/zpa/zdm_groups/{zdmGroupId}/versions", async () =>
            await client.GetZpaVersionsAsync("grp_123"));

        Console.WriteLine("\n--- H.323 / SIP Devices (4 ops) ---");
        await DomainRunner.RunOperationAsync("deviceList", "GET /h323/devices", async () =>
            await client.ListH323DevicesAsync(pageSize: 10));
        await DomainRunner.RunOperationAsync("deviceCreate", "POST /h323/devices", async () =>
            await client.CreateH323DeviceAsync(new CreateH323DeviceRequest { Name = "Boardroom H323", Ip = "192.168.1.100", Protocol = "H.323", Encryption = "auto" }));
        await DomainRunner.RunOperationAsync("deviceUpdate", "PATCH /h323/devices/{deviceId}", async () =>
            await client.UpdateH323DeviceAsync(dummyDeviceId, new UpdateH323DeviceRequest { Name = "Boardroom H323 Updated" }));
        await DomainRunner.RunOperationAsync("deviceDelete", "DELETE /h323/devices/{deviceId}", async () =>
            await client.DeleteH323DeviceAsync(dummyDeviceId));
    }
}
