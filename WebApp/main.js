// alert("test");

socket= new WebSocket('192.168.0.116:12345');
alert("sdf");
socket.onopen= function() {
    // socket.send('hello');
    alert("open");
};
socket.onmessage= function(s) {
    // alert('got reply '+s);
    alert("closed")
};