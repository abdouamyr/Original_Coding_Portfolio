Floor middleAppt;
Float oldYPos;
void setMaxHeight () {
  if (apartmentScale == 100) {
    maxCameraHeight = 100;
  } else if (apartmentScale == 200) {
    maxCameraHeight = 5400;
  } else if (apartmentScale == 400) {
    maxCameraHeight = 15200;
  }
  //  } else if (apartmentScale == 50) {
  //    maxCameraHeight = -2400;
  //  } else if (apartmentScale == 25) {
  //    maxCameraHeight = -3700;
  //  }
  //  if (cameraY < -1 *(apartments[49].refY * apartmentScale)) {
  //     cameraY = -1 * (apartments[49].refY * apartmentScale);
  //   }
}
void setCameraBounds () {
  if (cameraY > maxCameraHeight) {
    cameraY = maxCameraHeight;
  }
}

void findOnScreenApartment () {
  middleAppt = apartments[0];
  float middleFloor = abs(apartments[0].posY + cameraY - screenHeight/2);
  for (int i = 0; i < apartments.length; i++) {
    apartments[i].onScreen = false;
    if (middleFloor > abs(apartments[i].posY + cameraY - screenHeight/2)) {
      middleFloor =  abs(apartments[i].posY + cameraY - screenHeight/2);
      middleAppt = apartments[i];
    }
  }
  oldYPos = (float) middleAppt.posY;
  middleAppt.onScreen = true;
}

void scaleForZoom () {
  cameraY += oldYPos - (middleAppt.refY * apartmentScale);  
  //  if (apartmentScale > 50) {
  //    cameraY += oldYPos - (middleAppt.refY * apartmentScale);  
  //  } else if (apartmentScale <= 50) {
  //    cameraY -= (middleAppt.refY * 100) - oldYPos;  
  //  }
}

