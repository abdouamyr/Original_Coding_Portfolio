var imageFullscreen = false;
var divClosed = false;
var screenshots = [
  'Assets/Screen1.png',
  'Assets/Screen2.png',
  'Assets/Screen3.png',
  'Assets/Screen4.png',
  'Assets/Screen5.png',
  'Assets/Screen6.png'
];

var concepts = [
  'Assets/HatterConcept.png',
  'Assets/VillainConcept.jpg',
  'Assets/HatterAction.png',
  'Assets/HatterSMG.png',
  'Assets/HatterDualWielding.png',
  'Assets/HatterJumping.png'
];

var characters = [
  'Assets/HatterReveal.png',
  'Assets/NivaraReveal.png',
  'Assets/JaxReveal.png'
];

var characterBios = [
  "Hatter is a mysterious vigilante. She came to the city long ago and has been battling corruption ever since. Her signature weapons are her two 9mm, though she has recently taken to carrying twin MP5's when heavier fire power is needed. Though her past is a mystery to all, the hat she wears was inherited from her father, a famed detective. Hatter's primary mission is to locate her father's killer.",
  'Nivara is a fierce warlord, trained in the desert kingdom of Magdon. She journeyed to Chioban to seek power. Using a small army of mercenaries, she quickly performed a takeover of the local government. In place as the ruling general of the city, she imposes her harsh rule upon it. She will not hesitate to crush those who stand against her.',
  "Jax is a military deserter and weapons expert. Living in Chioban's underground, he befriended Hatter when she arrived in the city. Since then, he's been supplying her with guns and ammo to combat Nivara. But the time is coming soon when Jax himself must take a stand."
];
$(document).ready(function() {
    var images = [];
    $('.screenshot').mouseenter(function() {
      images = screenshots;
    }); 
    $('.conceptArt').mouseenter(function() {
      images = concepts;
    });
     $('.imgBox').click(function() {
         $('.imgBox').toggle(800);
         imageFullscreen = true;
         if (divClosed) {
            $('.mainImage').empty();
            $('.mainImage').toggle(800); 
         }
         if ($(this).is("#box1")) {
            $(".mainImage").append("<img src =" + images[0] + " height = '500'>");
            console.debug('Box1');  
         } else if ($(this).is("#box2")) {
             $(".mainImage").append("<img src =" + images[1] + " height = '500'>");
             console.debug('Box2');  
         } else if ($(this).is("#box3")) {
              $(".mainImage").append("<img src =" + images[2] + " height = '500'>");
              console.debug('Box3');  
         } else if ($(this).is("#box4")) {
              $(".mainImage").append("<img src =" + images[3] + " height = '500'>");
              console.debug('Box4');  
         } else if ($(this).is("#box5")) {
              $(".mainImage").append("<img src =" + images[4] + " height = '500'>");
              console.debug('Box5');  
         } else if ($(this).is("#box6")) {
              $(".mainImage").append("<img src =" + images[5] + " height = '500'>");
              console.debug('Box6');    
        }  
   });
  
  $('.mainImage').click(function() { 
      if (imageFullscreen) {
        console.debug('clicked');    
        $('.mainImage').toggle(1000);
        $('.imgBox').toggle(1000); 
        divClosed = true;
      }
  });

  $('.characterBox').click(function() {
    if($(this).is('#box1')) {
        $('#box2').toggle(800); 
        $('#box3').toggle(800); 
        $('#box1').empty(); 
        $("#box1").append("<img src =" + characters[0] + " height = '600'>");
        $('.mainImage').empty();
        $(".mainImage").append("<p>" + characterBios[0] + "</p>");
    } else if($(this).is('#box2')) {
        $('#box1').toggle(800); 
        $('#box3').toggle(800); 
        $('#box2').empty(); 
        $("#box2").append("<img src =" + characters[1] + " height = '600'>");
        $('.mainImage').empty();
        $(".mainImage").append("<p>" + characterBios[1] + "</p>");
    } else if($(this).is('#box3')) {
        $('#box1').toggle(800); 
        $('#box2').toggle(800); 
        $('#box3').empty(); 
        $("#box3").append("<img src =" + characters[2] + " height = '600'>");
        $('.mainImage').empty();
        $(".mainImage").append("<p>" + characterBios[2] + "</p>");
    }
  });

});
