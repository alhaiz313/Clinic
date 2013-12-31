function insert(item, user, request) {

    var User = tables.getTable('User');
    User.where({
        UserId: user.userId

    }).read({
        success: function (results) {
            if (results.length == 1) {


                console.log("user admin value  is  ", results[0].isadmin)
                if (results[0].isadmin) {
                    request.execute();
                }
                else {
                    request.respond(statusCodes.FORBIDDEN, 'You are not authorized');
                }
            }
        }
    });
    //console.log("user is  ", User);
    //console.log( "original ",user)



}