/************************************************************************************
**************** Validate to see if value is a integer ******************************
************************************************************************************/
function validNumber(obj, value, validData) {
    var test = !isNaN(value - 0) && value !== null && value !== "" && value !== false;

    if (test == true) {
        obj.css({ 'background': 'lightgreen' });
    }
    else {
        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    
    return validData;
}

/************************************************************************************
**************** Validate to see if value is a date *********************************
************************************************************************************/
function validDate(obj, value, validData) {
    var tempDate = new Date(value);

    // it is a date
    if (isNaN(tempDate)) {
        // date is not valid
        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}

/************************************************************************************
**************** Validate to see if value is a blank ********************************
************************************************************************************/
function validEntry(obj, value, validData)
{
    // it is a date
    if (value == "" || value == null) {
        // date is not valid
        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}

/************************************************************************************
**************** Validate to see if value is a U.S. Zip Code ************************
************************************************************************************/
function validZipCode(obj, value, validData) {

    // it is a date
    if (/^\d{5}(-\d{4})?$/.test(value) == false) {
        
        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}

/************************************************************************************
**************** Validate to see if value is a U.S. EIN Number with dash ************
************************************************************************************/
function validEIN(obj, value, validData) {

    // it is a date
    if (/^\d{2}-\d{7}$/.test(value) == false) {

        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}

/************************************************************************************
**************** Validate to see if value is a U.S. Phone # ************************
************************************************************************************/
function validPhone(obj, value, validData)
{
    if (/^(?:\(\d{3}\)|\d{3})(?: *- *)?\d{3}(?: *- *)?\d{4}$/.test(value) == false) {

        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}

/************************************************************************************
**************** Validate to see if value is a U.S. Phone # ************************
************************************************************************************/
function validEmail(obj, value, validData)
{
    if (/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/.test(value) == false) {

        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}


function validUsername(obj, value, validData) {
    if (/^\S{6,}$/.test(value) == false) {

        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}


/// <summary>
/// Verify that a users password is:
/// 1) Atleast 6 characters.
/// 2) Atleast 1 UPPERCASE letter.
/// 3) Atleast 1 lowercase letter.
/// 4) Atleast 1 numeric digit.
/// 5) Atleast 1 special character.
/// </summary>
function validInitialPassword(obj, value, validData)
{
    if (/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.{6,15})(?=.*[@#$%^&+=_!*-]).*$/.test(value) == false) {
    //if (/^(?=.*[0-9])(?=.*[_!@#$%^&\-*])(?=.*[a-z])(?=.*[A-Z]).{6,}$/.test(value) == false) {
        obj.css({ 'background': '#FF9494' });
        validData = false;
    }
    else {
        obj.css({ 'background': 'lightgreen' });
    }

    return validData;
}

/************************************************************************************
**************** Validate to see if passwords match up ******************************
************************************************************************************/
function validPassword(obj, value, obj2, value2, validData) {
    //alert("Validate Data: " + validData);
    try
    {
        if (value == value2)
        {
            obj2.css({ 'background': 'lightgreen' });
        }
        else
        {
            obj2.css({ 'background': '#FF9494' });
            validData = false;
        }
    }
    catch(e)
    {
        validData = false;
    }
    finally
    {
        return validData;
    }
}




