;; Definitions for shooter agents.
(ns pucks.agents.blue-grunt
  (:use [pucks globals util vec2D]
        [pucks.agents active]))
  
(defn blue-grunt-proposals [p]
  {:acceleration (if (not (empty? (filter :red-grunt (:sensed p))))
                   0
                   1)
   :rotation 
   (if (not (empty? (filter :torpedo (:sensed p))))
     (+ pi (direction->rotation (:position (first (filter :torpedo (:sensed p))))))
     (if (not (empty? (filter :red-grunt (:sensed p))))
          (if (:mobile (first (filter :red-grunt (:sensed p))))
            (direction->rotation (:position (first (filter :red-grunt (:sensed p)))))
            (+ (direction->rotation (:position (first (filter :red-grunt (:sensed p))))) (/ pi 2)))
          (if (not (empty? (filter :stone (:sensed p))))
            (let [stonedir (+v (apply avgv (map :position (filter :stone (:sensed p)))))] ;to navigate around stones 
              (+ pi (direction->rotation stonedir))) 
            (if (not (empty? (filter :blue-grunt (:sensed p))))
              (let [allydir (+v (apply avgv (map :position (filter :blue-grunt (:sensed p)))))] ;to navigate with the pack
                (direction->rotation allydir))
              (if (and (not (empty? (filter :vent (:sensed p)))) 
                       (< (:energy p) 75))
                (direction->rotation (:position (first (filter :vent (:sensed p)))))
              (direction->rotation (:velocity p)))))))
   :fire-torpedo (if (and (not (empty? (filter :red-grunt (:sensed p))))
                          (= (mod (:steps p) 20) 0)
                          (:mobile (first (filter :red-grunt (:sensed p)))))
                     true
                     false)})

(defn blue-grunt []
  (merge (active)
         {:blue-grunt true
          :proposal-function blue-grunt-proposals
          :color [0 0 255]
          :eye-color [0 0 200]}))