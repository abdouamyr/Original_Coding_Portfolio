class Floor {
  color roomColor;
  int population, floorNumber, posY, refY;
  boolean lowestFloor, elevator, onScreen;
  ArrayList<Person> tenants = new ArrayList<Person>();
  Floor (int y) {
    posY = y*originalApartmentScale;
    refY = y;
    floorNumber = 50 - y;
    population = (int) random(1, maxFloorPopulation);
    roomColor = (0);
    lowestFloor = false;
    elevator = false;
    onScreen = false;
    for (int i = 0; i < population; i++) {
      tenants.add(new Person (screenWidth/2 + (i * apartmentScale/8) - apartmentScale/3, posY + (int) (apartmentScale/2.5)));
    }
  }
  void drawFloor() {
    //    (apartmentHeight + cameraY - screenHeight)
    posY = refY * apartmentScale;
    if (lowestFloor) {
      roomColor = 255;
    } else {
      roomColor = 0;
    }
    fill(roomColor, 0, 0);
    rect(screenWidth/2, posY, apartmentScale, apartmentScale);
    fill(255);
    textSize(apartmentScale/10);
    text("Floor# " + floorNumber, screenWidth/2 - apartmentScale/3, posY - apartmentScale/3);
    text("Population: " + population, screenWidth/2 - apartmentScale/3, posY);
    //    text("posY: " + (posY), screenWidth/2 - apartmentScale/3, posY + apartmentScale/3);
    fill(0);
    for (int i = 0; i < population; i++) {
      updateHappiness (tenants.get(i));
      for (int j = 0; j < population; j++) {
        if (tenants.get(j) != tenants.get(i) && inBounds(tenants.get(i), tenants.get(j)) && !tenants.get(j).inConversation && !tenants.get(i).inConversation) {
          tenants.get(i).stop = true;
          tenants.get(i).inConversation = true;
          tenants.get(j).inConversation = true;
          tenants.get(j).stop = true;
        }
      }
      tenants.get(i).drawPerson(screenWidth/2 + (i * apartmentScale/8) - apartmentScale/3, posY + (int) (apartmentScale/2.5));
      if (tenants.get(i).happiness == 0 && elevator && elevator1.unloaded == false) {
        leaveApartment(tenants.get(i));
      }
    }
  }
  //    text("-- Height: " + (apartmentHeight - posY) * 100/apartmentScale, screenWidth/2 + apartmentScale/2, posY - apartmentScale/3);
  void leaveApartment (Person person) {
    elevator1.riders.add(person);
    //    Person [] newTenants = new Person[population - 1];
    //    int j = 0;
    for (int i = 0; i < population; i++) {
      if (i < tenants.size() && tenants.get(i) == person) {
        tenants.get(i).inElevator = true;
        tenants.get(i).stop = true;
        tenants.remove(i);
      }
    }
    //    tenants = newTenants;
    population--;
  }
  boolean inBounds(Person personA, Person personB) {
    if (apartmentScale/8 > abs(personA.x - personB.x) && apartmentScale/32 < abs(personA.x - personB.x)) {
      return true;
    } else {
      return false;
    }
  }

  boolean leftOut (Person personA) {
    if (!personA.inConversation) {
      for (int j = 0; j < population; j++) {
        if (!tenants.get(j).inConversation && tenants.get(j) != personA) {
          return false;
        }
      }
      return true;
    } else {
      return false;
    }
  }

  void updateHappiness (Person person) {
    if (((population == 1 || population > 5)&& person.happiness > 0 && !person.inConversation) || leftOut(person)) {
      person.happiness-= 0.25;
    }
    if ((person.inConversation  || population%2 == 0)&& person.happiness < 100) {
      person.happiness+= 1.5;
    }
  }
}

