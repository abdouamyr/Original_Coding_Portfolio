(ns super-mash.core)

(def spouses '("Oprah Winfrey" "Sean Connery" "Conan O'Brein" "Tom Cruise" "Roald Dahl" "Tom Sawyer" "Donald Duck" "Mickey Mouse" "Darth Vader" "Wonder Woman" "Spiderman" "Mary Jane" "Batman" "Mario" "Princess Peach" "Sarah Palin"))
(def meet-cute '("fishing for trout in the Hudson" "skydiving for the first time" "taking a singles cooking class" "walking contemplatively on the beach" "they're saving each other from sharks" "dealing out vigilante justice"))
(def houses '("a mansion" "The Bat Cave" "a shed" "a broom cupboard" "Hogwars" "Stark Tower" "The Daily Bugle"))
(def jobs '("Dragon Slayer" "Tax Accountant" "Programmer" "Zookeeper" "Box Office Representative" "Photographer"))
(def num-kids '(1.5 2 3 4 5 6 7 8 9 10 0 100 1000 10000 "infinite"))
(def type-kids '("children" "dogs" "goats" "monkeys" "troll dolls" "woolen sweaters" "stamps" "lambs" "chia pets" "presidents"))
(def death-cause '("is stabbed by a clown" "falls off a skyscraper" "tries to armwrestle Superman" "passes away while sleeping" "spontaneously combusts"))
(def death-age '(20 30 37 45 53 61 72 89 94 101))
(def attract-adj '("insant" "intense" "sultry" "gradual" "inequivalent"))
(def age-adj '("ripe old" "shocking" "tender" "laughable"))
(def live-adv '("happily" "joyously" "unhappily" "comfortably" "contentdedly"))
(def move-adv '("quicky" "soon" "slowly" "laboriously" "delightedly" "begrudingly"))
(def kid-future '("grow up to be geniuses" "go on to star in their own reality tv series" "are collectively smarter than a rock" "all have to repeat the 5th grade" "turn out be psychotic murderers" "start a very successful boy band"))

(defn my-future [my-name]
  (print (str my-name " will meet " (rand-nth spouses) " while " (rand-nth meet-cute)".") "\n" 
         (str " The attraction will be " (rand-nth attract-adj) " and they will " (rand-nth move-adv) " move into " (rand-nth houses) ".") "\n"
         (str " Here they will raise " (rand-nth num-kids) " " (rand-nth type-kids) " who " (rand-nth kid-future) ".") " \n"
         (str " They'll live together " (rand-nth live-adv) " until " my-name " "(rand-nth death-cause) " at the " (rand-nth age-adj) " age of " (rand-nth death-age)".") 
         ))

