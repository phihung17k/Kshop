import { DownOutlined } from "@ant-design/icons";
import { Container } from "@material-ui/core";
import Paper from "@material-ui/core/Paper";
import { makeStyles } from "@material-ui/core/styles";
import Tab from "@material-ui/core/Tab";
import Tabs from "@material-ui/core/Tabs";
import { Dropdown, Menu, Space } from "antd";
import React from "react";
import "./Filter.css";
const Filter = (props) => {
  const UseStyles = makeStyles({
    root: {
      flexGrow: 1,
      marginBottom:'45px'
    },
  });
  const style = {
    dropDown: {
      display: "flex",
      alignItems: "center",
      justifyContent: "right",
    },
  };
  const useStyles = makeStyles((theme) => ({
    formControl: {
      margin: theme.spacing(1),
      minWidth: 120,
    },
    selectEmpty: {
      marginTop: theme.spacing(2),
    },
  }));
  const classes = UseStyles();
  const [value, setValue] = React.useState(0);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  const menu = (
    <Menu>
      <Menu.Item>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="https://www.antgroup.com"
        >
          Price
        </a>
      </Menu.Item>
      <Menu.Item>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="https://www.antgroup.com"
        >
          Date
        </a>
      </Menu.Item>
      <Menu.Item>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="https://www.antgroup.com"
        >
          Quanity
        </a>
      </Menu.Item>
    </Menu>
  );
  return (
   
      <Paper className={classes.root}>
        <Tabs
          value={value}
          onChange={handleChange}
          TabIndicatorProps={{
            style: {
              backgroundColor: "#DFD6AF",
              color: "#000",
            },
          }}
          left
        >
          <Tab label="ALL" />
          <Tab label="BESTSELLER" />
          <Tab label="NEW IN" />
          
        </Tabs>
      </Paper>
    
  );
};

Filter.propTypes = {};

export default Filter;
