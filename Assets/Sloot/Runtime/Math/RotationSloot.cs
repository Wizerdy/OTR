using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sloot {
    public class RotationSloot : MonoBehaviour {
        #region Axis
        public enum Turn {
            CLOCKWISE,
            ANTICLOCKWISE,
            NEAR
        }

        public static Vector3 GetVectorDownIfDefault(Vector3 axis) {
            if (axis == default) {
                axis = Vector3.down;
            }
            return axis;
        }

        public static Vector3 GetBasicOriginDirection(Vector3 axis) {
            if (axis == default) {
                axis = Vector3.forward;
            }
            Vector3 origin = Vector3.zero;
            switch (axis) {
                case Vector3 v when v.Equals(Vector3.forward):
                    origin = Vector3.right;
                    break;
                case Vector3 v when v.Equals(Vector3.back):
                    origin = Vector3.left;
                    break;
                case Vector3 v when v.Equals(Vector3.right):
                    origin = Vector3.back;
                    break;
                case Vector3 v when v.Equals(Vector3.left):
                    origin = Vector3.forward;
                    break;
                case Vector3 v when v.Equals(Vector3.up):
                    origin = Vector3.right;
                    break;
                case Vector3 v when v.Equals(Vector3.down):
                    origin = Vector3.right;
                    break;
                default:
                    throw new ArgumentException("Vector not recognise", nameof(origin));
                    break;

            }
            return origin;
        }

        /// <summary>
        /// Returns a vector which indicate the direction where an object should be to be at X <paramref name="degrees"/> degrees of the origin (0/0/0) around the <paramref name="axis"/>.
        /// </summary>
        /// <param name = "degrees" > Value must be in degrees </param>
        /// <param name = "axis" > Vector3.down is the default </param>
        public static Vector3 GetDirectionOnAxis(float degrees, Vector3 axis = default) {
            Modulo360(degrees);
            axis = GetVectorDownIfDefault(axis);
            Vector3 aroundAxis = GetBasicOriginDirection(axis);
            return Quaternion.Euler(axis.normalized * degrees) * aroundAxis;
        }

        /// <summary>
        /// Returns a vector which indicate the position where an object should be to be at X <paramref name="degrees"/> and X <paramref name="distance"/> of <paramref name="origin"/> around the <paramref name="axis"/>.
        /// </summary>
        /// <param name = "degrees" > Value must be in degrees </param>
        /// <param name = "axis" > Vector3.down is the default </param>
        public static Vector3 GetPosOnAxis(Vector3 origin, float degrees, float distance, Vector3 axis = default) {
            return origin + GetDirectionOnAxis(degrees, axis) * distance;
        }


        #endregion
        #region Angle
        public static float TWOPI = Mathf.PI * 2;
        public static float Modulo360(float toModulo) {
            return (toModulo % 360 + 360) % 360;
        }

        public static float ModuloTwoPI(float toModulo) {
            return (toModulo % TWOPI + TWOPI) % TWOPI;
        }

        public static float Lap360(float degree, int lap) {
            return Modulo360(degree) + 360 * lap;
        }

        public static float LapTwoPI(float radian, int lap) {
            return ModuloTwoPI(radian) + TWOPI * lap;
        }

        #endregion
        public static Vector3 PutVectorOnPlan(Vector3 vector, Vector3 axis = default) {
            axis = GetVectorDownIfDefault(axis);
            switch (axis) {
                case Vector3 v when v.Equals(Vector3.forward):
                    vector.z = 0;
                    break;
                case Vector3 v when v.Equals(Vector3.back):
                    vector.z = 0;
                    break;
                case Vector3 v when v.Equals(Vector3.right):
                    vector.x = 0;
                    break;
                case Vector3 v when v.Equals(Vector3.left):
                    vector.x = 0;
                    break;
                case Vector3 v when v.Equals(Vector3.up):
                    vector.y = 0;
                    break;
                case Vector3 v when v.Equals(Vector3.down):
                    vector.y = 0;
                    break;
                default:
                    throw new ArgumentException("Vector not recognise", nameof(axis));
            }
            return vector;
        }

        /// <summary>
        /// Returns a float which indicate the angle in radian of <paramref name="target"/> around <paramref name="position"/> using the <paramref name="axis"/> and the <paramref name="originAxis"/> as angle value 0 of <paramref name="axis"/>.
        /// </summary>
        /// <param name = "axis" > Vector3.down is the default </param>
        /// <param name = "originAxis" > Vector3.right is the default </param>
        public static float GetRadBasedOfTarget(Vector3 position, Vector3 target, Vector3 axis = default, Vector3 originAxis = default) {
            axis = GetVectorDownIfDefault(axis);
            if (originAxis == default || Vector3.Dot(axis, originAxis) != 0) {
                originAxis = GetBasicOriginDirection(axis);
            }
            Vector3 positionTarget = target - position;
            positionTarget = PutVectorOnPlan(positionTarget, axis);
            float DotProduct = Vector3.Dot(originAxis.normalized, positionTarget);
            Vector3 CrossProduct = Vector3.Cross(originAxis.normalized, positionTarget.normalized);
            if (Vector3.Dot(axis, CrossProduct.normalized) == -1) {
                return -Mathf.Acos(DotProduct / positionTarget.magnitude) + TWOPI;
            } else {
                return Mathf.Acos(DotProduct / positionTarget.magnitude);
            }
        }

        /// <summary>
        /// Returns a float which indicate the angle in degrees of <paramref name="target"/> around <paramref name="position"/> using the <paramref name="axis"/> and the <paramref name="originAxis"/> as angle value 0 of <paramref name="axis"/>.
        /// </summary>
        /// <param name = "axis" > Vector3.down is the default </param>
        /// <param name = "originAxis" > Vector3.right is the default </param>
        public static float GetDegreeBasedOfTarget(Vector3 position, Vector3 target, Vector3 axis = default, Vector3 originAxis = default) {
            return GetRadBasedOfTarget(position, target, axis, originAxis) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Move <paramref name="toTurn"/> around <paramref name="toTurnAround"/> at <paramref name="angularSpeed"/> in order to reach <paramref name="degrees"/> using the <paramref name="axis"/> and the direction given by <paramref name="type"/>.
        /// </summary>
        /// <param name = "degrees" > Value must be in degrees </param>
        /// <param name = "angularSpeed" > 
        /// <br> Value must be in degrees per second</br>
        /// <br> The value "360" is equal to one turn every second</br>
        /// </param>
        /// <param name = "axis" > Vector3.down is the default </param>
        /// <param name = "originAxis" > Vector3.right is the default </param>
        public static IEnumerator GoAtDegreeAroundTransform(Transform toTurn, Transform toTurnAround, Vector3 axis, float degrees, float angularSpeed, Turn type) {
            degrees = Modulo360(degrees);
            float currentDegree = GetDegreeBasedOfTarget(toTurnAround.position, toTurn.position, axis);
            if (currentDegree == degrees) {
                yield break;
            }
            float radius = Vector3.Distance(toTurn.position, toTurnAround.position);
            float goAtDegree = 0f;
            bool clockwise = true;
            switch (type) {
                case Turn.CLOCKWISE:
                    if (currentDegree < degrees) {
                        goAtDegree = degrees - 360;
                    } else {
                        goAtDegree = degrees;
                    }
                    clockwise = true;
                    break;
                case Turn.ANTICLOCKWISE:
                    if (currentDegree > degrees) {
                        goAtDegree = degrees + 360;
                    } else {
                        goAtDegree = degrees;
                    }
                    clockwise = false;
                    break;
                case Turn.NEAR:
                    if (currentDegree > degrees) {
                        if (Mathf.Abs(currentDegree - degrees) < Mathf.Abs(currentDegree - (degrees + 360))) {
                            goAtDegree = degrees;
                            clockwise = true;
                        } else {
                            goAtDegree = degrees + 360;
                            clockwise = false;
                        }
                    } else {
                        if (Mathf.Abs(currentDegree - degrees) < Mathf.Abs(currentDegree - (degrees - 360))) {
                            goAtDegree = degrees;
                            clockwise = false;
                        } else {
                            goAtDegree = degrees - 360;
                            clockwise = true;
                        }
                    }
                    break;
            }
            if (clockwise) {
                while (currentDegree > goAtDegree) {
                    currentDegree -= Time.deltaTime * angularSpeed;
                    if (currentDegree > goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround.position, currentDegree, radius, axis);
                    } else {
                        toTurn.position = GetPosOnAxis(toTurnAround.position, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            } else {
                while (currentDegree < goAtDegree) {
                    currentDegree += Time.deltaTime * angularSpeed;
                    if (currentDegree < goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround.position, currentDegree, radius, axis);
                    } else {
                        toTurn.position = GetPosOnAxis(toTurnAround.position, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            }
        }
        /// <summary>
        /// Move <paramref name="toTurn"/> around <paramref name="toTurnAround"/> at <paramref name="angularSpeed"/> during <paramref name="degree"/> using the <paramref name="axis"/>.
        /// </summary>
        /// <param name = "degrees" > Value must be in degrees </param>
        /// <param name = "angularSpeed" > 
        /// <br> Value must be in degrees per second</br>
        /// <br> The value "360" is equal to one turn every second</br>
        /// </param>
        public static IEnumerator TurnDegreeAroundTransform(Transform toTurn, Transform toTurnAround, Vector3 axis, float degree, float angularSpeed) {
            float currentDegree = GetDegreeBasedOfTarget(toTurnAround.position, toTurn.position, axis);
            float radius = Vector3.Distance(toTurn.position, toTurnAround.position);
            int direction = currentDegree < currentDegree + degree ? 1 : -1;
            float goAtDegree = currentDegree + degree;
            if (direction == 1) {
                while (currentDegree < goAtDegree) {
                    currentDegree += Time.deltaTime * angularSpeed;
                    toTurn.position = GetPosOnAxis(toTurnAround.position, currentDegree, radius, axis);
                    if (currentDegree >= goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround.position, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            } else {
                while (currentDegree > goAtDegree) {
                    currentDegree -= Time.deltaTime * angularSpeed;
                    toTurn.position = GetPosOnAxis(toTurnAround.position, currentDegree, radius, axis);
                    if (currentDegree <= goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround.position, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            }
        }
    }
}
