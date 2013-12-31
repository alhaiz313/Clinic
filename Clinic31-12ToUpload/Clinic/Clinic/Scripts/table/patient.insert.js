var azure = require('azure');
var qs = require('querystring');
var appSettings = require('mobileservice-config').appSettings;
var HttpStatusOk = 200;
var HttpStatusBadRequest = 400;
var HttpStatusForbidden = 403;
var HttpStatusNotFound = 404;
//var devices = tables.getTable('device');

function insert(item, user, request) {
    // Get storage account settings from app settings. 
    var accountName = appSettings.STORAGE_ACCOUNT_NAME;
    var accountKey = appSettings.STORAGE_ACCOUNT_ACCESS_KEY;
    var host = accountName + '.blob.core.windows.net';


    if ((typeof item.containerName !== "undefined") && (
    item.containerName !== null)) {
        // Set the BLOB store container name on the item, which must be lowercase.
        item.containerName = item.containerName.toLowerCase();


        // If it does not already exist, create the container 
        // with public read access for blobs.        
        var blobService = azure.createBlobService(accountName, accountKey, host);
        blobService.createContainerIfNotExists(item.containerName, {
            publicAccessLevel: 'blob'
        }, function (error) {
            if (!error) {


                // Provide write access to the container for the next 5 mins.        
                var sharedAccessPolicy = {
                    AccessPolicy: {
                        Permissions: azure.Constants.BlobConstants.SharedAccessPermissions.WRITE,
                        Expiry: new Date(new Date().getTime() + 5 * 60 * 1000)
                    }
                };


                // Generate the upload URL with SAS for the new image.
                var sasQueryUrl =
                blobService.generateSharedAccessSignature(item.containerName,
                item.resourceName, sharedAccessPolicy);


                // Set the query string.
                item.sasQueryString = qs.stringify(sasQueryUrl.queryString);


                // Set the full path on the new new item, 
                // which is used for data binding on the client. 
                item.imageUri = sasQueryUrl.baseUrl + sasQueryUrl.path;


            } else {
                console.error(error);
            }
            request.execute();
            sendNotifications();
        });
    } else {
        request.execute();
        sendNotifications();
    }

    function sendNotifications() {

        var channelsTable = tables.getTable('Device');
        channelsTable.read({
            success: function (devices) {
                devices.forEach(function (device) {
                    push.wns.sendToastText04(device.channelUri, {
                        text1: item.text
                    }, {
                        success: function (pushResponse) {
                            console.log("Sent push:", pushResponse);
                        }
                    });
                });
            }
        });
        //var devicesTable = tables.getTable('Device');
        //devicesTable.read({
        //    success: function (devices) {
        //        devices.forEach(function (device) {
        //            push.wns.sendToastText04(device.channelUri, {
        //                text1: item.text
        //            }, {
        //                success: function (pushResponse) {
        //                    console.log("Sent push:", pushResponse);
        //                },
        //                error: function (err) {

        //                    // The notification address for this device has expired, so
        //                    // remove this device. This may happen routinely as part of
        //                    // how push notifications work.
        //                    if (err.statusCode === HttpStatusForbidden || err.statusCode === HttpStatusNotFound) {
        //                        devices.del(device.id);
        //                    } else {
        //                        console.log("Problem sending push notification", err);
        //                    }
        //                }
        //            });
        //        });
        //    }
        //});
        //devices.read({
        //    success: function (results) {
        //        results.forEach(function (device) {
        //            push.wns.sendToastText04(device.channelUri, {
        //                text1: item.text
        //            }, {
        //                success: function (pushResponse) {
        //                    console.log("Sent push:", pushResponse);
        //                }
        //            });
        //        });
        //    }
        //});



        //devices.read({
        //    success: function(results) {
        //        results.forEach(function(device) {
        //            push.wns.sendToastText04(device.channelUri, {
        //                text1: item.text
        //            }, {
        //                success: function(pushResponse) {
        //                    console.log("Sent push:", pushResponse);
        //                }
        //            });
        //        });
        //    }
        //});
    }



}