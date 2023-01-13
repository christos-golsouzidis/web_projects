var canvas = document.getElementById("canvas");
var context = canvas.getContext("2d");

var seed = 0x11335577;
var maxval = 0xffffffff;

document.getElementById("seed").innerHTML = seed;

function draw_values() {
  for (var x = 0; x < 1536; x += 1) {
    seed = nextrand();
    if (!(seed & 1)) {
      context.fillRect(x, y, 1, 1);
    }
  }
  y += 1;
  if (y < 512) {
    draw_values();
  }
}

var y = 0;
draw_values();

//--- write here the peudorandom function u want to test...

function nextrand() {
  seed ^= seed << 13;
  seed &= 0xffffffff;
  seed ^= seed >>> 17;
  seed ^= seed << 5;
  seed &= 0xffffffff;

  return seed;
}
