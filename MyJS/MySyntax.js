
function test1() {
  var v1 = "v1"; // 局部变量
  v2 = "v2"; // 全局变量
}

// test1();
// test2("123");
// test2("123","22");
// alert(v2);
// alert(v1);
// alert(typeof null);
// alert(typeof undefined);
// alert(typeof test1);
// alert(typeof v3); // undefined
// var v4 = null;
// alert(typeof v4); // object
// alert(v4 == undefined); // true
// alert(v4 === undefined); // false
// alert(0.1 + 0.2)
// alert(Number.parseInt("1b"));
// alert(Number("1b"));
// alert( NaN > 'A');
// function test2(p1, p2) {
//   // alert(arguments[0])
//   // alert(p1)
//   // p1 = "77"
//   // alert(arguments[0])
//   // alert(p1)
//   // p2 = "88"
//   // alert(arguments[1])
//   // alert(p2)
//   alert("两个参数");
// }
// function test2(p2){
//   alert("一个参数");
// }


// function f1(p1){
//   p1 = new Object()
//   p1.value = "2";
// }
// var o1 = new Object();
// o1.value = "1";
// f1(o1);
// alert(o1.value);


function f1(){
  with(location){
    var t1 = "11";
  }
  alert(t1);
}
f1();