(ns pucks.maneuvers (:use [pucks globals util vec2D]
        [pucks.agents active]))
 
(defn avoid
  [self other]
  (if (< 
        (Math/abs (- (:rotation self) (+ (/ pi 2) (direction->rotation (:position other))))) 
        (Math/abs (- (:rotation self) (- (/ pi 2) (direction->rotation (:position other))))))
    (+ (/ pi 2) (direction->rotation (:position other)))
    (- (/ pi 2) (direction->rotation (:position other)))))

(defn vec-avoid 
[self vector]
(let 
  [rotation (direction->rotation vector)]
 (if (< 
        (Math/abs (- (:rotation self) (+ (/ pi 2) rotation))) 
        (Math/abs (- (:rotation self) (- (/ pi 2) rotation))))
    (+ pi rotation)
    (- pi rotation))))
(defn follow 
  [puck]
  (direction->rotation (:position puck)))

;from the swarmer puck
(defn rand-direction []
  (rotation->direction (- (rand two-pi) pi)))