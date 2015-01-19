boolean overlap (int x1, int y1, int x2, int y2, int radius) {
  if ( abs(x1 - x2) < radius && abs(y1 - y2) < radius) {
    return true;
  } else {
    return false;
  }
}

boolean collide (int x1, int y1, int x2, int y2, String check, int radius) {
  if (check == "x" &&
    radius/4 > abs(x1 - x2) && radius*1.5 > abs(y1- y2)) {
    return true;
  } else if (check == "y" &&
    radius/4 > abs(y1 - y2) && radius*1.5 > abs(x1- x2)) {
    return true;
  } else {
    return false;
  }
}
