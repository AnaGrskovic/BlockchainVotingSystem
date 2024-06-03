export class GraphModel {
    Value: number;
    Color: string;
    Size: string;
    Legend: string;
  
    constructor(value: number = 0, color: string = '', size: string = '', legend: string = '') {
      this.Value = value;
      this.Color = color;
      this.Size = size;
      this.Legend = legend;
    }
}
  