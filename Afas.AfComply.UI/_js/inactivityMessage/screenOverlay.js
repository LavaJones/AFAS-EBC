function skm_LockScreen(str)
{
    var lock = document.getElementById('skm_LockPane');
    var lock2 = document.getElementById('');

    if (lock)
        lock.className = 'LockOn';

    lock.innerHTML = str;
}