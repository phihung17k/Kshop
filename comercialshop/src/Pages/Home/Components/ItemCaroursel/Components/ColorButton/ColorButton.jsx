import React from 'react'
import PropTypes from 'prop-types'
import Button from '@material-ui/core/Button';
import { grey } from "@material-ui/core/colors";
import { withStyles } from "@material-ui/core/styles";
import Box from '@material-ui/core/Box';
const ColorButton = props => {
    const ColorButton = withStyles((theme) => ({
        root: {
            color: theme.palette.getContrastText(grey[900]),
            backgroundColor: grey[900],
            "&:hover": {
                backgroundColor: grey[800]
            },
            borderRadius: '0px'

        }
    }))(Button);
    const style={
        button:{fontWeight: '300'}
    }
    return (
        <ColorButton variant="contained" color="primary" >
            <Box letterSpacing={3} style={style.button}>
                DISCOVER COLLECTION
                    </Box>
        </ColorButton>
    )
}

ColorButton.propTypes = {

}

export default ColorButton
