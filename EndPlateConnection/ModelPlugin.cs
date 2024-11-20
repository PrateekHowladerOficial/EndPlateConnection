using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Plugins;

using static Tekla.Structures.Model.Position;
using Identifier = Tekla.Structures.Identifier;
using Line = Tekla.Structures.Geometry3d.Line;
using Fitting = Tekla.Structures.Model.Fitting;

using TeklaPH;

namespace EndPlateConnection

{
    public class PluginData
    {
        #region Fields
        [StructuresField("FlagBolt")]
        public int FlagBolt;

        [StructuresField("FlagWasher1")]
        public int FlagWasher1;

        [StructuresField("FlagWasher2")]
        public int FlagWasher2;

        [StructuresField("FlagWasher3")]
        public int FlagWasher3;

        [StructuresField("FlagNut1")]
        public int FlagNut1;

        [StructuresField("FlagNut2")]
        public int FlagNut2;

        [StructuresField("Thickness")]
        public int Thickness;
        #endregion
    }

    [Plugin("EndPlateConnection")]
    [PluginUserInterface("EndPlateConnection.MainForm")]
    public class EndPlateConnection : PluginBase
    {
        Model myModel = new Model();
        #region Fields
        private Model _Model;
        private PluginData _Data;
        //
        // Define variables for the field values.
        //
        /* Some examples:
        private string _RebarName = string.Empty;
        private string _RebarSize = string.Empty;
        private string _RebarGrade = string.Empty;
        private ArrayList _RebarBendingRadius = new ArrayList();
        private int _RebarClass = new int();
        private double _RebarSpacing;
        */
        #endregion

        #region Properties
        private Model Model
        {
            get { return this._Model; }
            set { this._Model = value; }
        }

        private PluginData Data
        {
            get { return this._Data; }
            set { this._Data = value; }
        }
        #endregion

        #region Constructor
        public EndPlateConnection(PluginData data)
        {
            Model = new Model();
            Data = data;
        }
        #endregion

        #region Overrides
        public override List<InputDefinition> DefineInput()
        {
            List<InputDefinition> PointList = new List<InputDefinition>();
            Picker Picker = new Picker();
            var part = Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_OBJECT, "Pick Primary object");
            var partno1 = part;
            PointList.Add(new InputDefinition(partno1.Identifier));
            part = Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_OBJECT, "Pick Secondary object");
            var partno2 = part;
            PointList.Add(new InputDefinition(partno2.Identifier));
            return PointList;
        }

        public override bool Run(List<InputDefinition> Input)
        {
            try
            {
                GetValuesFromDialog();

                Beam beam1 = myModel.SelectModelObject(Input[0].GetInput() as Identifier) as Beam;
                Beam beam2 = myModel.SelectModelObject(Input[1].GetInput() as Identifier) as Beam;


                Point origin1 = beam1.EndPoint;
                var girtCoord = beam1.GetCoordinateSystem();
                girtCoord.Origin = origin1;
                //girtCoord.AxisX = girtCoord.AxisX *- 1;

                TransformationPlane currentTransformation = myModel.GetWorkPlaneHandler().GetCurrentTransformationPlane();
                var newWorkPlane = new TransformationPlane(girtCoord);
                // workPlaneHandler.SetCurrentTransformationPlane(newWorkPlane);
                myModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(newWorkPlane);

                //main logic start 


                //workPlaneHandler.SetCurrentTransformationPlane(currentTransformation);
                myModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(currentTransformation);
            }
            catch (Exception Exc)
            {
                MessageBox.Show(Exc.ToString());
            }

            return true;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the values from the dialog and sets the default values if needed
        /// </summary>
        private void GetValuesFromDialog()
        {
            /* Some examples:
            _RebarName = Data.RebarName;
            _RebarSize = Data.RebarSize;
            _RebarGrade = Data.RebarGrade;

            char[] Parameters = { ' ' };
            string[] Radiuses = Data.RebarBendingRadius.Split(Parameters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string Item in Radiuses)
                _RebarBendingRadius.Add(Convert.ToDouble(Item));

            _RebarClass = Data.RebarClass;
            _RebarSpacing = Data.RebarSpacing;

            if (IsDefaultValue(_RebarName))
                _RebarName = "REBAR";
            if (IsDefaultValue(_RebarSize))
                _RebarSize = "12";
            if (IsDefaultValue(_RebarGrade))
                _RebarGrade = "Undefined";
            if (_RebarBendingRadius.Count < 1)
                _RebarBendingRadius.Add(30.00);
            if (IsDefaultValue(_RebarClass) || _RebarClass <= 0)
                _RebarClass = 99;
            if (IsDefaultValue(_RebarSpacing) || _RebarSpacing <= 0)
                _RebarSpacing = 200.0;
            */
        }

        // Write your private methods here.

        #endregion
    }
}
