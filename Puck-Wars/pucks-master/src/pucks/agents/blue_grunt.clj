;; Definitions for shooter agents.
(ns pucks.agents.blue-grunt
  (:use [pucks globals util vec2D maneuvers]
        [pucks.agents active]
        [clojure.set]))
  
(defn blue-grunt-proposals [p]
  (let 
    [enemies (concat (filter #(= (:mobile %) true) (filter :red-grunt (:sensed p)))
                     (filter #(= (:mobile %) true) (filter :red-medic (:sensed p))))
    allies (concat (filter #(= (:mobile %) true) (filter :blue-grunt (:sensed p)))
                   (filter #(= (:mobile %) true) (filter :blue-medic (:sensed p))))
    squad (filter #(= (:squad-number %) (:squad-number p)) allies)
    allydir (+v (apply avgv (map :position allies)))
    squad-dir (+v (apply avgv (map :position squad)))
    the-dead (concat (filter #(= (:mobile %) false) (filter :red-grunt (:sensed p)))
                     (filter #(= (:mobile %) false) (filter :blue-grunt (:sensed p)))
                     (filter #(= (:mobile %) false) (filter :blue-medic (:sensed p)))
                     (filter #(= (:mobile %) false) (filter :red-medic (:sensed p))))
    bullets (filter :torpedo (:sensed p))
    bulletdir (+v (apply avgv (map :position bullets)))
    rich-vents (filter #(> (:energy %) 0.25) (filter :vent (:sensed p)))
    dry-vents (filter #(< (:energy %) 0.25) (filter :vent (:sensed p)))
    stones (filter :stone (:sensed p))
    turrets (concat (filter :blue-gunner (:sensed p))
                    (filter :red-gunner (:sensed p)))
    stonedir (+v (apply avgv (map :position stones)))
    obstacledir  (+v (apply avgv (map :position (concat (concat stones turrets) dry-vents))))]
    {
     :acceleration (if (or(not (empty? enemies))
                          (not (empty? bullets)))
                     1
                     0.25)
     :rotation 
     (if (not (empty? bullets))
       (vec-avoid p bulletdir)
       (if (not (empty? enemies))
         (follow (first enemies))
         (if (not (empty? the-dead))
           (avoid p (first the-dead))
           (if (and (not (empty? rich-vents)) 
                    (< (:energy p) 0.75))
                 (follow (first rich-vents))
                 (if (not (empty? (concat stones turrets)))
                   (vec-avoid p obstacledir) ;to avoid stones 
                   (if (not (empty? squad))
                     (direction->rotation (+v squad-dir (rand-direction))) ;to navigate with the pack
                     (direction->rotation (:velocity p))))))))
     :fire-torpedo (if (and (not (empty? enemies))
                            (= (mod (:steps p) 5) 0))
                     true
                     false)}))

(defn blue-grunt [squad-num]
  (merge (active)
         {:blue-grunt true
          :proposal-function blue-grunt-proposals
          :color [0 0 (- 255 (* squad-num 50))]
          :eye-color [0 0 200]
          :squad-number squad-num}))