
// Set timeout variables.
var timoutWarning = 140000; // Display warning in 19 Mins.
var timoutNow = 200000; // Timeout in 1 mins.

var warningTimer;
var timeoutTimer;

// Start timers.
function StartTimers() {
    warningTimer = setTimeout("IdleWarning()", timoutWarning);
    timeoutTimer = setTimeout("IdleTimeout()", timoutNow);
}

// Reset timers.
function ResetTimers() {
    clearTimeout(warningTimer);
    clearTimeout(timeoutTimer);
    StartTimers();
    $("#timeout").dialog('close');
}

// Show idle timeout warning dialog.
function IdleWarning() {
    $("#timeout").dialog({
        modal: true
    });
}

// Logout the User.
function IdleTimeout() {
    //alert("timeout");
    document.getElementById("BtnLogout").click();
}