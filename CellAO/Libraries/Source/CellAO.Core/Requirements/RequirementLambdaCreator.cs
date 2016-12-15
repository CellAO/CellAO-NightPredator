#region License

// Copyright (c) 2005-2016, CellAO Team
// 
// 
// All rights reserved.
// 
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

#endregion

namespace CellAO.Core.Requirements
{
    #region Usings ...

    using System;
    using System.Linq.Expressions;

    using CellAO.Core.Entities;
    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    public static class RequirementLambdaCreator
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> BitAndExpression()
        {
            return (i, i1) => (i & i1) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> BitOrExpression()
        {
            return (i, i1) => (i | i1) != 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="t">
        /// </param>
        /// <param name="o">
        /// </param>
        /// <param name="statId">
        /// </param>
        /// <param name="statValue">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static Expression<Func<IInstancedEntity, bool>> BuildExpression(
            ItemTarget t,
            Operator o,
            int statId,
            int statValue)
        {
            switch (o)
            {
                case Operator.GreaterThan:
                case Operator.LessThan:
                case Operator.EqualTo:
                case Operator.TestNumPets:
                case Operator.BitAnd:
                case Operator.NotBitAnd:
                case Operator.BitOr:
                case Operator.Unequal:
                case Operator.Not:
                    Func<int, int, bool> tmp = SwitchStatOperator((int)o).Compile();
                    Func<IInstancedEntity, IInstancedEntity> tmp2 = GetTarget(t).Compile();
                    return k => tmp.Invoke(tmp2.Invoke(k).Stats[statId].Value, statValue);
                case Operator.HasNotFormula:

                    return k => ((Character)GetTarget(t).Compile().Invoke(k)).HasNano(statValue);
                case Operator.FlyingAllowed:
                    return k => true;
                default:

                    throw new NotImplementedException("Operator " + o.ToString() + " not implemented yet.");
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> EqualExpression()
        {
            return (i, i1) => i == i1;
        }

        /// <summary>
        /// </summary>
        /// <param name="statId">
        /// </param>
        /// <returns>
        /// </returns>
        public static Expression<Func<IInstancedEntity, int>> GetStatValExpression(int statId)
        {
            return (k) => k.Stats[statId].Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="t">
        /// </param>
        /// <returns>
        /// </returns>
        public static Expression<Func<IInstancedEntity, IInstancedEntity>> GetTarget(ItemTarget t)
        {
            switch (t)
            {
                case ItemTarget.Self:
                    return (k) => k;
                case ItemTarget.Fightingtarget:
                    return (k) => ((ITargetingEntity)k).Playfield.FindByIdentity(((ITargetingEntity)k).FightingTarget);
                case ItemTarget.Target:
                    return (k) => ((ITargetingEntity)k).Playfield.FindByIdentity(((ITargetingEntity)k).SelectedTarget);
                case ItemTarget.User:
                    return (k) => k;
                case ItemTarget.Wearer:
                    return (k) => k;
                default:
                    return (k) => k;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> GreaterThanExpression()
        {
            return (i, j) => i > j;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> LessThanExpression()
        {
            return (i, j) => i < j;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> NotBitAndExpression()
        {
            return (i, i1) => (i & i1) == 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="op">
        /// </param>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> SwitchStatOperator(int op)
        {
            switch (op)
            {
                case (int)Operator.GreaterThan:
                    return GreaterThanExpression();
                case (int)Operator.LessThan:
                    return LessThanExpression();
                case (int)Operator.EqualTo:
                    return EqualExpression();
                case (int)Operator.TestNumPets:
                    return LessThanExpression();
                case (int)Operator.BitAnd:
                    return BitAndExpression();
                case (int)Operator.NotBitAnd:
                    return NotBitAndExpression();
                case (int)Operator.BitOr:
                    return BitOrExpression();
                case (int)Operator.Unequal:
                    return UnequalExpression();
                case (int)Operator.Not:
                    return UnequalExpression();
                default:
                    return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<bool>> TrueExpression()
        {
            return () => true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Expression<Func<int, int, bool>> UnequalExpression()
        {
            return (i, i1) => i != i1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="requirements">
        /// </param>
        /// <returns>
        /// </returns>
        internal static Func<IInstancedEntity, bool> Create(Requirement requirements)
        {
            return
                BuildExpression(
                    (ItemTarget)requirements.Target,
                    (Operator)requirements.Operator,
                    requirements.Statnumber,
                    requirements.Value).Compile();
        }

        #endregion
    }
}