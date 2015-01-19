;; Definitions for healer agents.
(ns pucks.agents.red-medic
  (:use [pucks globals util vec2D maneuvers]
        [pucks.agents active]
        [clojure.set]))
  
(defn red-medic-proposals [p]
  (let 
    [enemies (concat (filter #(= (:mobile %) true) (filter :blue-grunt (:sensed p)))
                     (filter #(= (:mobile %) true) (filter :blue-medic (:sensed p))))
    allies (concat (filter #(= (:mobile %) true) (filter :red-grunt (:sensed p)))
                   (filter #(= (:mobile %) true) (filter :red-medic (:sensed p))))
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
     :acceleration (if (not (empty? rich-vents))
                     1
                     0.25)
     :rotation 
     (if (not (empty? bullets))
       (vec-avoid p bulletdir)
       (if (and (not (empty? squad))
                (> (:energy p) 0.25))
         (direction->rotation (+v squad-dir (rand-direction))) ;to navigate with the pack
         (if (and (not (empty? rich-vents)) 
                  (< (:energy p) 0.75))
           (follow (first rich-vents))
           (if (not (empty? the-dead))
             (avoid p (first the-dead))
             (if (not (empty? enemies))
               (avoid p (first enemies))
               (if (not (empty? (concat stones turrets)))
                 (vec-avoid p obstacledir) ;to avoid stones                             
                 (direction->rotation (:velocity p))))))))
     :transfer (into [] (for [recipient (filter #(< (:energy %) 0.5) allies)]
               {:self (:id p)
                :other (:id recipient)
                :bid {:energy 0.01}
                :ask {}}))}))

(defn red-medic [squad-num]
  (merge (active)
         {:red-medic true
          :proposal-function red-medic-proposals
          :color [(- 255 (* squad-num 50)) 0 0]
          :eye-color [0 255 0]
          :squad-number squad-num}))